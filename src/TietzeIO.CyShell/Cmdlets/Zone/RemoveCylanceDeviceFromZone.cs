using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Zone
{
    [Cmdlet(VerbsCommon.Remove, "CylanceDeviceFromZone")]
    public class RemoveCylanceDeviceFromZone : CyParallelPipelineCmdlet<object, CyDeviceMetaData>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public CyDeviceMetaData Device { get; set; }
        [Parameter(Mandatory = false)]
        public CyZone[] Zone;

        private Guid[] _zoneIds;

        public RemoveCylanceDeviceFromZone() : base()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_ADD_REMOVE_DEVICES_IN_ZONES,
                "Removing devices from zones");
        }


        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Debug.Assert(Zone != null);
            _zoneIds = (from z in Zone select z.id).ToArray();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            WriteVerbose($"Removing device {Device.device_id} from zones \"{string.Join(", ", _zoneIds)}\"");
            QueueObject(Device);
        }

        protected override Task[] CreateTasksFromObjects(CyDeviceMetaData[] q)
        {
            return new[] { Connection.RemoveDeviceFromZoneAsync(Device, _zoneIds) };
        }
    }
}
