using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Cache
{
    [Cmdlet(VerbsCommon.Get, "CylanceCachedThreats")]
    [OutputType(typeof(CyThreatMetaData))]
    public class GetCylanceCachedThreats : CyCachedModeCmdlet
    {
        public GetCylanceCachedThreats()
        {
            RequireProtectCache = true;
            RequireOpticsCache = false;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Api.Threats.ForEach(WriteObject);
        }
    }
}
