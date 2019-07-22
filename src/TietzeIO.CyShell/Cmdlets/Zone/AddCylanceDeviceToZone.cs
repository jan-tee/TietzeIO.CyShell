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
    [Cmdlet(VerbsCommon.Add, "CylanceDeviceToZone")]
    public class AddCylanceDeviceToZone : CyParallelPipelineCmdlet<object, CyDeviceMetaData>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public CyDeviceMetaData Device { get; set; }
        [Parameter(Mandatory = false)]
        public CyZone[] Zone;

        private Guid[] _zoneIds;

        public AddCylanceDeviceToZone() : base()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_ADD_REMOVE_DEVICES_IN_ZONES,
                "Adding devices to zones");
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Debug.Assert(Zone != null);
            _zoneIds = (from z in Zone select z.id).ToArray();
        }

        protected override Task[] CreateTasksFromObjects(CyDeviceMetaData[] q)
        {
            return new[] { Connection.AddDeviceToZoneAsync(Device, _zoneIds) };
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            WriteVerbose($"Adding device {Device.device_id} to zones \"{string.Join(", ", _zoneIds)}\"");
            QueueObject(Device);
        }
    }
}
