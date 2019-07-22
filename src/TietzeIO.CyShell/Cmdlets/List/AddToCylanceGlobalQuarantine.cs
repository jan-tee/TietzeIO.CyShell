using System.Management.Automation;
using System.Net;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.List
{
    [Cmdlet(VerbsCommon.Add, "ToCylanceGlobalQuarantineList")]
    [OutputType(typeof(CyRestAddToGlobalList))]
    public class AddToCylanceGlobalQuarantineList : CyThreatCmdlet
    {

        [Parameter(Mandatory = true)]
        public string Reason;

        public AddToCylanceGlobalQuarantineList() : base()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GLOBAL_LIST_OP,
                "Adding to global quarantine list");
        }

        protected override Task[] CreateTasksFromObjects(string[] q)
        {
            return new[]{AttemptAndSoftFail(
                AddHash(q),
                new [] { $"Could not add {q[0]}: Hash already in list" },
                new [] { HttpStatusCode.Conflict })};
        }

        private async Task<object> AddHash(string[] q)
        {
            Cancellation.ThrowIfCancellationRequested();
            WriteVerbose($"Adding {q[0]} to global quarantine list.");
            var r = await Connection.AddToGlobalQuarantineListAsync(q[0], Reason);
            return r;
        }
    }
}
