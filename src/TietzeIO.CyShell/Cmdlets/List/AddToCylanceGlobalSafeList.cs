using System.Management.Automation;
using System.Net;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.List
{
    [Cmdlet(VerbsCommon.Add, "ToCylanceGlobalSafeList")]
    [OutputType(typeof(CyRestAddToGlobalList))]
    public class AddToCylanceGlobalSafeList : CyThreatCmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateSet("Admin Tool", "Drivers", "Internal Application", "Operating System", "Security Software", "None")]
        public string Category { get; set; }

        [Parameter(Mandatory = true)]
        public string Reason;

        public AddToCylanceGlobalSafeList() : base()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GLOBAL_LIST_OP,
                "Adding to global safe list");
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
            WriteVerbose($"Adding {q[0]} to global safe list.");
            var r = await Connection.AddToGlobalSafeListAsync(q[0], Category, Reason);
            return r;
        }
    }
}
