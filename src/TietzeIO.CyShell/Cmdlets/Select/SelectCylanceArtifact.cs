using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Select
{
    [Cmdlet(VerbsCommon.Select, "CylanceArtifact")]
    [OutputType(typeof(CyDetection.CyArtifact))]
    public class SelectCylanceArtifact : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyDetection Detection { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet("Instigating Process", "Instigating Process Image File", "Target Process", "Target Process Image File", "Instigating Process Owner", "Target Process Owner", "Target File", "Target Network Connection")]
        public string[] Source { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet("File", "Process", "User", "NetworkConnection")]
        public string[] Type { get; set; }

        [Parameter(Mandatory = false)]
        public string[] State { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (Detection != null)
            {
                var matchingAois =
                    new ArtifactsOfInterestFilter()
                    {
                        StateFilter = State,
                        SourceFilter = Source,
                        TypeFilter = Type
                    }.Filter(Detection);
                if (matchingAois.Count > 0)
                {
                    // there are AOIs that pass the filter
                    matchingAois.ForEach(WriteObject);
                }
            }
        }
    }
}
