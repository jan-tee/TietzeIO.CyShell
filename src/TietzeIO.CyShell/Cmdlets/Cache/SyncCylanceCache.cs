using System.Management.Automation;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Cache
{
    [Cmdlet(VerbsData.Sync, "CylanceCache")]
    public class SyncCylanceCache : CyCachedModeCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Api.RefreshCache(Connection, this);
        }
    }
}
