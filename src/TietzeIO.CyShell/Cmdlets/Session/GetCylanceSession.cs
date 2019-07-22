using System.Management.Automation;
using TietzeIO.CyShell.Session;

namespace TietzeIO.CyShell.Cmdlets.Session
{
    [Cmdlet(VerbsCommon.Get, "CylanceSession")]
    [OutputType(typeof(ApiConnectionHandle))]
    public class GetCylanceSession : Cmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            WriteObject(ApiConnectionHandle.Global);
        }
    }
}
