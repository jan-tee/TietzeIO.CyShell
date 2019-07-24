using Nito.AsyncEx.Synchronous;
using System.Management.Automation;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Device
{
    [Cmdlet(VerbsCommon.Get, "CylanceDevices")]
    [OutputType(typeof(CyDeviceBase))]
    public class GetCylanceDevices : CyApiCmdlet
    {
        [Parameter(Mandatory = false)]
        public CyZone Zone;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (null == Zone) // no filter, get all
                Connection.
                    RequestDeviceListAsync().
                    WaitAndUnwrapException().
                    ForEach(WriteObject);
            else // filter by zone
                Connection.
                    RequestDeviceListForZoneAsync(Zone.id).
                    WaitAndUnwrapException().
                    ForEach(WriteObject);
        }
    }
}
