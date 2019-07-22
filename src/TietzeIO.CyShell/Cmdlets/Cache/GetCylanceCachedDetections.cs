using System.Management.Automation;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Cache
{
    [Cmdlet(VerbsCommon.Get, "CylanceCachedDetections")]
    [OutputType(typeof(ICyDetection))]
    public class GetCylanceCachedDetections : CyCachedModeCmdlet
    {
        [Parameter]
        public SwitchParameter Detailed;

        public GetCylanceCachedDetections()
        {
            RequireProtectCache = false;
            RequireOpticsCache = true;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (Detailed.IsPresent)
            {
                Api.DetectionsDetail.ForEach(WriteObject);
            }
            else
            {
                Api.Detections.ForEach(WriteObject);
            }
        }
    }
}
