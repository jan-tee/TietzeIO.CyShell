using System.Diagnostics;
using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Detection
{
    [Cmdlet(VerbsCommon.Open, "CylanceDetection")]
    public class OpenCylanceDetection : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyDetectionMetaData Detection { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (Detection != null)
            {
                var url = ApiV2.GetShardUrl(ShardUrl.PROTECT, "euc1", $"/Optics#/detect/{Detection.id}");
                Process.Start(url);
            }
        }
    }
}
