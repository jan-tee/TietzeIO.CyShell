using System.Management.Automation;
using System.Net;
using System.Threading.Tasks;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Threat
{
    [Cmdlet(VerbsSecurity.Block, "CylanceDeviceThreat")]
    [OutputType(typeof(CyThreatDevice))]
    public class BlockCylanceDeviceThreat : CyParallelPipelineCmdlet<object, ICyThreatInstance>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public ICyThreatInstance ThreatInstance { get; set; }

        public BlockCylanceDeviceThreat()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_BLOCK_THREAT,
                "Blocking threat(s)");
            EnableOutput = false;
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            QueueObject(ThreatInstance);
        }

        protected override Task[] CreateTasksFromObjects(ICyThreatInstance[] q)
        {
            return new[] {
                AttemptAndSoftFail(
                    BlockThreat(q),
                    new [] {"Threat instance not found.", "Threat instance status could not be set (Conflict)"},
                    new [] { HttpStatusCode.NotFound, HttpStatusCode.Conflict })
            };
        }

        private async Task<object> BlockThreat(ICyThreatInstance[] instance)
        {
            Cancellation.ThrowIfCancellationRequested();
            WriteVerbose($"Locally quarantining threat {instance[0].sha256} on device {instance[0].device_id}.");
            var r = await Connection.QuarantineDeviceThreatAsync(instance[0]);
            return r;
        }
    }
}
