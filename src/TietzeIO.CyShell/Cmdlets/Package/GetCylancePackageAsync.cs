using Nito.AsyncEx.Synchronous;
using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Detection
{
    [Cmdlet(VerbsCommon.Get, "CylancePackage")]
    [OutputType(typeof(CyPackage))]

    public class GetCylancePackage : CyParallelPipelineCmdlet<CyPackage, CyPackageMetaData>
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = "ByPackage")]
        public CyPackageMetaData Package { get; set; }

        public GetCylancePackage()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GET_DETECTION, "Downloading package");
        }

        protected override Task[] CreateTasksFromObjects(CyPackageMetaData[] q)
        {
            return new[] {
                Connection.GetPackageAsync(q[0].packageId)
            };
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            QueueObject(Package);
        }
    }
}
