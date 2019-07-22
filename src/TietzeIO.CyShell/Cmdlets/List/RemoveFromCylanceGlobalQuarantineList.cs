using System.Management.Automation;
using System.Net;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.List
{
    [Cmdlet(VerbsCommon.Remove, "FromCylanceGlobalQuarantineList")]
    [OutputType(typeof(CyRestRemoveFromGlobalList))]
    public class RemoveFromCylanceGlobalQuarantineList : CyThreatCmdlet
    {
        public RemoveFromCylanceGlobalQuarantineList() : base()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GLOBAL_LIST_OP,
                "Removing from global quarantine list");
        }

        protected override Task[] CreateTasksFromObjects(string[] q)
        {
            return new[]{
                AttemptAndSoftFail(
                RemoveHash(q),
                new [] { $"Could not remove {q[0]}: Hash not found" },
                new [] { HttpStatusCode.NotFound }) };
        }

        private async Task<object> RemoveHash(string[] q)
        {
            Cancellation.ThrowIfCancellationRequested();
            WriteVerbose($"Removing {q[0]} from global quarantine list.");
            var r = await Connection.RemoveFromGlobalQuarantineListAsync(q[0]);
            return r;
        }
    }
}
