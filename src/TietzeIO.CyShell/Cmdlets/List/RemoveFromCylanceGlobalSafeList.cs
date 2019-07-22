using System.Management.Automation;
using System.Net;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.List
{
    [Cmdlet(VerbsCommon.Remove, "FromCylanceGlobalSafeList")]
    [OutputType(typeof(CyRestRemoveFromGlobalList))]
    public class RemoveFromCylanceGlobalSafeList : CyThreatCmdlet
    {
        public RemoveFromCylanceGlobalSafeList() : base()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GLOBAL_LIST_OP,
                "Removing from global safe list");
        }
        protected override Task[] CreateTasksFromObjects(string[] q)
        {
            return new[] { AttemptAndSoftFail<object>(
                RemoveHashes(q),
                new [] { $"Could not remove {q[0]}: Hash not found" },
                new [] { HttpStatusCode.NotFound }) };
        }

        protected async Task<object> RemoveHashes(string[] q)
        {
            Cancellation.ThrowIfCancellationRequested();
            WriteVerbose($"Removing {q[0]} from global safe list.");
            var r = await Connection.RemoveFromGlobalSafeListAsync(q[0]);
            return r;
        }
    }
}
