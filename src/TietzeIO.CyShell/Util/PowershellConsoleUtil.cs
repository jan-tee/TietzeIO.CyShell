using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util.Progress;

namespace TietzeIO.CyShell.Util
{
    public class PowershellConsoleUtil
    {
        /// <summary>
        /// Shows a progress indicator while waiting for a set of asynchronous tasks to complete.
        /// </summary>
        /// <typeparam name="T">Type of result for the tasks</typeparam>
        /// <param name="cmdlet">cmdlet to display progress information</param>
        /// <param name="tasks">Tasks to wait for completion</param>
        /// <param name="activityKey">PSCmdlet acitivity key</param>
        /// <param name="activity">Activity "headline"</param>
        /// <param name="statusDescription">Status description</param>
        public static void WaitForTasksToCompleteWithProgress2<T>(CyCmdlet cmdlet, List<Task<T>> tasks, int activityKey, string activity, string statusDescription)
        {
            var p = new System.Management.Automation.ProgressRecord(activityKey, activity, statusDescription);

            int c;
            int t;
            do
            {
                c = tasks.Where(task => task.IsCompleted).Count();
                t = tasks.Count;
                p.PercentComplete = (int)(c * 100f / t);
                cmdlet.WriteProgress(p);
                Thread.Sleep(20);
                cmdlet.Logger.FlushLogsToPowershellConsole();
            } while (c < t);
        }

        public static void BlockWithProgress(CyCmdlet cmdlet, List<Task> tasks, int activityKey, string activity, string statusDescription)
        {
            var progressInfoCalculator = new PowershellProgressInfoCalculator(activityKey, activity, tasks);
            do
            {
                progressInfoCalculator.CalculateProgress();
                progressInfoCalculator.WriteProgress(cmdlet);
                Task.Delay(100).Wait();
                cmdlet.Logger.FlushLogsToPowershellConsole();
            } while (!tasks.TrueForAll(x => x.IsCompleted));
        }
    }
}
