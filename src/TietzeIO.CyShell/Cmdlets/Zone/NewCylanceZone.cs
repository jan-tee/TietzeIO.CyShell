using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Zone
{
    [Cmdlet(VerbsCommon.New, "CylanceZone")]
    [OutputType(typeof(CyZone))]
    public class NewCylanceZone : CyApiCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }
        [Parameter(Mandatory = true)]
        public CyPolicyMinimalMetaData Policy { get; set; }
        [Parameter(Mandatory = false)]
        [ValidateSet("Low", "Medium", "High")]
        public string Criticality { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            var zone = Connection.CreateZoneAsync(Name, Policy, Criticality).WaitAndUnwrapException();
            if (null != zone) WriteObject(zone);
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
