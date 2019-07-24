using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TietzeIO.CyShell.Util.Progress;


namespace TietzeIO.CyShell.Cmdlets.Base
{
    // TODO: This is a prototype for moving from task-based queuing with a large number of waiting tasks to object-based queuing with a small number of waiting tasks to improve efficiency


    /// <summary>
    /// An abstract class to build Powershell Cmdlets that support asynchronous, multi-threaded
    /// mass operations on data, while preserving the order of source objects and associated results.
    /// </summary>
    /// <typeparam name="TOutput">Output type for a task. E.g. if you wish to queue tasks that
    /// operate on strings, and the result is the number of character in each strings, then this
    /// would most likely be integer.</typeparam>
    /// <typeparam name="TWorkItem">The object type for the work items, when you are generating tasks from
    /// objects. E.g. if you wish to queue tasks that operate on strings as input objects, this would
    /// be string.</typeparam>
    public abstract class CyParallelPipelineCmdlet2<TOutput, TWorkItem> : CyApiCmdlet where TOutput : new()
    {
        // the queue of objects that tasks will be created from. This is separate from the Tasks queue, because there may not be a 1:1 but 1:100 or so mapping.
        private Queue<TWorkItem> InputObjectsQueue;
        private Queue<Task> TasksQueue; // this is used to be able to return output in correct sequence.
        private List<Task> AllTasks; // this will have every task
        protected bool EnumerateCollectionsInOutput { get; set; }

        public long MaxItemsInTaskQueue { get; set; } = 8;

        // when set, is used to calculate overall progress and display it
        private PowershellProgressInfoCalculator _progressInfo;

        // the number of objects to remove from the Queue when creating a Task
        protected int ObjectsPerTransaction { get; set; } = 1;

        // whether "output" objects from completed tasks should be sent to output
        public bool EnableOutput { get; set; } = true;

        protected CyParallelPipelineCmdlet2() : base()
        {
            EnumerateCollectionsInOutput = false;
            InputObjectsQueue = new Queue<TWorkItem>(100);
            TasksQueue = new Queue<Task>(1000);
            AllTasks = new List<Task>(1000);
        }

        /// <summary>
        /// Queues a task.
        ///
        /// This method can be used directly when a task that was not created by from an object automatically (QueueObject) needs
        /// to be added to the work items.
        /// </summary>
        /// <param name="task"></param>
        public void QueueTask(Task task)
        {
            TasksQueue.Enqueue(task);
            AllTasks.Add(task);
        }

        /// <summary>
        /// Enables Powershell progress bars/messages for this activity.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public void EnableProgressInfo(int key, string name)
        {
            _progressInfo = new PowershellProgressInfoCalculator(key, name, AllTasks);
        }

        private void DropStatusMessage(bool force = false)
        {
            if (null != _progressInfo)
            {
                _progressInfo.CalculateProgress(force);
                _progressInfo.WriteProgress(this);
            }
        }

        private bool IsActive(Task t)
        {
            return t.Status == TaskStatus.WaitingForActivation ||
                t.Status == TaskStatus.WaitingForChildrenToComplete ||
                t.Status == TaskStatus.WaitingToRun ||
                t.Status == TaskStatus.Running ||
                t.Status == TaskStatus.Created;
        }

        /// <summary>
        /// Queues an object. Typically called by ProcessRecord in derived classes to enqueue an object.
        /// </summary>
        /// <param name="o"></param>
        protected void QueueObject(TWorkItem o)
        {
            if (!Stopping)
            {
                WriteVerbose($"Queuing object: {o}");
                InputObjectsQueue.Enqueue(o);
                if ((InputObjectsQueue.Count >= ObjectsPerTransaction) && (TasksQueue.Count() < MaxItemsInTaskQueue))
                {
                    WriteVerbose($"High water threshold of {ObjectsPerTransaction} reached, AND less than {MaxItemsInTaskQueue} in queue. Creating new task. Object queue length: {InputObjectsQueue.Count}. Task queue length: {TasksQueue.Count}");

                    while (InputObjectsQueue.Count >= ObjectsPerTransaction)
                    {
                        WriteVerbose($"Converting {ObjectsPerTransaction} objects to tasks.");
                        TWorkItem[] objectsToTaskify = new TWorkItem[ObjectsPerTransaction];
                        for (int i = 0; i < ObjectsPerTransaction; i++) objectsToTaskify[i] = InputObjectsQueue.Dequeue();

                        var tasksFromObjects = CreateTasksFromObjects(objectsToTaskify);
                        if (null != tasksFromObjects)
                        {
                            foreach (var task in tasksFromObjects)
                            {
                                QueueTask(task);
                            }
                        }
                        WriteVerbose($"New object queue length: {InputObjectsQueue.Count}. Task queue length: {TasksQueue.Count}");
                    }
                }
                WriteObjectsFromCompletedTasks();
            }
            DropStatusMessage();
        }

        /// <summary>
        /// When operating on input objects, this will be called once for every 'ObjectsPerTransaction' elements in the
        /// 'InputObjectsQueue' and returns the tasks to be added to the Queue.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        protected virtual Task[] CreateTasksFromObjects(TWorkItem[] q)
        {
            throw new NotImplementedException("CreateTasksFromObjects MUST be overriden");
        }

        /// <summary>
        /// Check for any completed tasks, and write objects. Non-blocking! If no completed tasks are available, returns immediately.
        /// </summary>
        protected void WriteObjectsFromCompletedTasks()
        {
            // check if first element of queue has completed, and return record.
            // repeat until first element is not completed, or no more elements.
            long flushed = FlushResultObjects(false);
            if (flushed > 0)
                WriteVerbose($"ProcessRecord: Flushed {flushed} records");
        }

        /// <summary>
        /// Flushes all results. Blocking or non-blocking flavors available.
        /// </summary>
        /// <returns></returns>
        private long FlushResultObjects(bool block)
        {
            long flushed = 0;

            do
            {
                while ((TasksQueue.Count > 0) && (TasksQueue.Peek().IsCompleted))
                {
                    var task = TasksQueue.Dequeue();
                    if (task.IsCompleted)
                    {
                        flushed++;
                        if (EnableOutput)
                        {
                            var t = task as Task<TOutput>;
                            if ((t != null) && (t.Result != null))
                            {
                                var r = ResultObjectTransformation(t.Result);
                                if (r != null)
                                {
                                    WriteObject(r, EnumerateCollectionsInOutput);
                                }
                            }
                        }
                    }
                    Logger.FlushLogsToPowershellConsole();
                }
                if (block)
                {
                    Task.Delay(10);
                }
                DropStatusMessage();
            } while (block && (TasksQueue.Count > 0));

            return flushed;
        }

        /// <summary>
        /// Override this to transform the output object before it is returned
        /// </summary>
        /// <param name="o">output object before transformation</param>
        /// <returns>transformed output object</returns>
        protected virtual TOutput ResultObjectTransformation(TOutput o)
        {
            return o;
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            if (InputObjectsQueue.Count == 0)
            {
                WriteVerbose($"EndProcessing: No remaining records to be converted to tasks.");
            }
            else
            {
                WriteVerbose($"EndProcessing: Creating remaining tasks for {InputObjectsQueue.Count} remaining records.");
                var tasksFromObjects = CreateTasksFromObjects(InputObjectsQueue.ToArray());
                if (null != tasksFromObjects)
                {
                    foreach (var task in tasksFromObjects)
                    {
                        QueueTask(task);
                    }
                }
                InputObjectsQueue.Clear();
            }
            WriteVerbose($"EndProcessing: Waiting for up to {TasksQueue.Count} remaining records");
            long flushed = FlushResultObjects(true);
            WriteVerbose($"EndProcessing: Flushed {flushed} additional records");
            DropStatusMessage(true);
            WriteObjectsFromCompletedTasks();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            WriteObjectsFromCompletedTasks();
        }
    }
}
