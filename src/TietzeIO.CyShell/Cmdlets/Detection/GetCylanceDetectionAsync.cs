using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Detection
{
    [Cmdlet(VerbsCommon.Get, "CylanceDetection")]
    [OutputType(typeof(CyDetection))]
    public class GetCylanceDetection : CyParallelPipelineCmdlet<CyDetection, CyDetectionMetaData>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyDetectionMetaData Detection { get; set; }

        public GetCylanceDetection()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GET_DETECTION, "Downloading detection detail");
        }

        protected override Task[] CreateTasksFromObjects(CyDetectionMetaData[] q)
        {
            return new[] {
                Connection.GetDetectionAsync(q[0].id)
            };
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            QueueObject(Detection);
        }
    }
}
