using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Select
{
    [Cmdlet(VerbsCommon.Select, "CylanceThreat")]
    [OutputType(typeof(CyThreatMetaData))]
    public class SelectCylanceThreat : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyThreatMetaData Threat { get; set; }

        [Parameter(Mandatory = false)]
        public string[] Classification;
        [Parameter(Mandatory = false)]
        public string[] SubClassification;
        [Parameter(Mandatory = false)]
        public string[] Name;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (Threat != null)
            {
                if (ThreatFilter.FilterThreat(Threat, Classification, SubClassification, Name))
                {
                    WriteObject(Threat);
                }
            }
        }
    }
}
