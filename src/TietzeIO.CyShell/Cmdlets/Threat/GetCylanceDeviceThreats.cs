using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Threat
{
    [Cmdlet(VerbsCommon.Get, "CylanceDeviceThreats")]
    [OutputType(typeof(CyThreatDevice))]
    public class GetCylanceDeviceThreats : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public CyDeviceMetaData Device { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (Device != null)
                WriteObject(Connection.RequestDeviceThreatsAsync(Device).WaitAndUnwrapException());
        }

    }
}
