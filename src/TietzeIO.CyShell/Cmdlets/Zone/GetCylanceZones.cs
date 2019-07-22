using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Zone
{
    [Cmdlet(VerbsCommon.Get, "CylanceZones")]
    [OutputType(typeof(CyZone))]
    public class GetCylanceZones : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                RequestZoneListAsync().
                WaitAndUnwrapException().
                ForEach(WriteObject);
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
