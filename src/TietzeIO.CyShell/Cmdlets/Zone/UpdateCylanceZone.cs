using System.Diagnostics;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Zone
{
    [Cmdlet(VerbsData.Update, "CylanceZone")]
    [OutputType(typeof(CyZone))]
    public class UpdateCylanceZone : CyApiCmdlet
    {
        [Parameter(Mandatory = true)]
        public CyZone Zone { get; set; }

        [Parameter(Mandatory = false)]
        public string Name { get; set; }
        [Parameter(Mandatory = false)]
        public CyPolicyMinimalMetaData Policy { get; set; }
        [Parameter(Mandatory = false)]
        [ValidateSet("Low", "Medium", "High")]

        public string Criticality { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Debug.Assert(Zone.id != null);
            Connection.UpdateZoneAsync(Zone, Name, Policy, Criticality).WaitAndUnwrapException();
        }
    }
}
