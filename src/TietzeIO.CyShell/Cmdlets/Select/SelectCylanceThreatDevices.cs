using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Select
{
    [Cmdlet(VerbsCommon.Select, "CylanceThreatDevices")]
    [OutputType(typeof(CyThreatDevice))]
    public class SelectCylanceThreatDevices : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyThreatDevice Threat { get; set; }

        [Parameter(Mandatory = false)]
        public string[] Classification;
        [Parameter(Mandatory = false)]
        public string[] SubClassification;
        [Parameter(Mandatory = false)]
        public string[] Name;
        [Parameter(Mandatory = false)]
        public string[] LiteralPath;
        //[Parameter(Mandatory = false)]
        //public string[] GlobPath;
        //[Parameter(Mandatory = false)]
        //public string[] RegexPath;

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if (Threat != null)
            {
                if (ThreatFilter.FilterThreatDevice(Api.Threats, Threat, Classification, SubClassification, Name, LiteralPath))
                {
                    WriteObject(Threat);
                }
            }
        }
    }
}
