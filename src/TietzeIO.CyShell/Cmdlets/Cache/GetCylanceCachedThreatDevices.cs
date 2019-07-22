using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Cache
{
    [Cmdlet(VerbsCommon.Get, "CylanceCachedThreatDevices")]
    [OutputType(typeof(CyThreatDevice))]
    public class GetCylanceCachedThreatDevices : CyCachedModeCmdlet
    {
        public GetCylanceCachedThreatDevices()
        {
            RequireProtectCache = true;
            RequireOpticsCache = false;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Api.ThreatDevices.ForEach(WriteObject);
        }
    }
}
