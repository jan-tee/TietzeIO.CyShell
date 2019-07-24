using System.Collections.Concurrent;
using System.Management.Automation;
using System.Threading;

namespace TietzeIO.CyShell.Cmdlets.Base
{
    /// <summary>
    /// A thread-safe logger that is attached to APIv2 instances to allow for asynchronous logging from other threads.
    /// </summary>
    public class PowershellThreadSafeVerboseLogger
    {
        public class Message
        {
            public string RenderedMessage { get; private set; }
            public Message(string msg)
            {
                RenderedMessage = msg;
            }
        }

        // the log queue
        private ConcurrentQueue<Message> Log = new ConcurrentQueue<Message>();

        // the thread that is allowed to flush log entries
        private Thread cmdletThread;
        private PSCmdlet cmdlet;

        public PowershellThreadSafeVerboseLogger(PSCmdlet clet)
        {
            cmdletThread = Thread.CurrentThread;
            cmdlet = clet;
        }

        public void LogVerbose(string message)
        {
            Log.Enqueue(new Message(message));
            FlushLogsToPowershellConsole();
        }

        public void FlushLogsToPowershellConsole()
        {
            if (Thread.CurrentThread.Equals(cmdletThread))
            {
                while (Log.TryDequeue(out var log))
                {
                    // this is not 100% safe since WriteVerbose should only be called from ProcessRecord, EndProcessing, StopProcessing, BeginProcessing, but it's good enough in practice, for now...
                    cmdlet.WriteVerbose(log.RenderedMessage);
                }
            }
        }
    }
}
