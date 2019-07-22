using System.Management.Automation;
using System.Net;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Threat
{
    [Cmdlet(VerbsCommon.Get, "CylanceThreat")]
    [OutputType(typeof(CyThreat))]
    public class GetCylanceThreat : CyThreatCmdlet
    {
        public GetCylanceThreat()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GET_THREAT, "Retrieving threat details");
        }

        protected override Task[] CreateTasksFromObjects(string[] q)
        {
            return new[] {
                AttemptAndSoftFail(
                GetThreat(q),
                    new [] { $"Could not get {q[0]}: Threat not found" },
                    new [] { HttpStatusCode.NotFound }) };
        }

        private async Task<object> GetThreat(string[] q)
        {
            Cancellation.ThrowIfCancellationRequested();
            WriteVerbose($"Retrieving {q[0]}.");
            var r = await Connection.RequestThreatDetailsAsync(q[0]);
            return r;
        }
    }
}
