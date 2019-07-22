using System.Management.Automation;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Cache
{
    [Cmdlet(VerbsCommon.Clear, "CylanceCache")]
    public class ClearCylanceCache : CyCachedModeCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Api.ClearCache(this);
        }
    }
}
