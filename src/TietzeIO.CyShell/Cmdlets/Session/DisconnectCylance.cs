using System.Management.Automation;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Session;

namespace TietzeIO.CyShell.Cmdlets.Session
{
    [Cmdlet(VerbsCommunications.Disconnect, "Cylance")]
    [OutputType(typeof(ApiConnectionHandle))]
    public class DisconnectCylance : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (ApiConnectionHandle.Global != null)
            {
                WriteVerbose("Removing session");
                ApiConnectionHandle.ClearGlobal();
            }
            else
            {
                WriteVerbose("No session to disconnect");
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }
    }
}
