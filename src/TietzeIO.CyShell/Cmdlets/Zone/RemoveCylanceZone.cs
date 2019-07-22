using System.Diagnostics;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Zone
{
    [Cmdlet(VerbsCommon.Remove, "CylanceZone")]
    public class RemoveCylanceZone : CyApiCmdlet
    {
        [Parameter(Mandatory = true)]
        public CyZone Zone { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Debug.Assert(Zone.id != null);
            Connection.RemoveZoneAsync(Zone).WaitAndUnwrapException();
        }
    }
}
