using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Device
{
    [Cmdlet(VerbsCommon.Remove, "CylanceDevice")]
    public class RemoveCylanceDevice : CyParallelPipelineCmdlet<CyRestDeviceRemovalResponse, Guid>, IDynamicParameters
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDevice")]
        public CyDeviceBase Device { get; set; }

        // number of items to group together in one transaction. API limit is 20.
        private const int MAX_UPDATES_PER_TRANSACTION = 20;

        private PowershellDynamicParameter DeviceNameParam;
        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;
            DeviceNameParam = new PowershellDynamicParameter(this, "Name", (from p in Api.Devices select p.device_name).ToArray());
            DeviceNameParam.ParameterSetName = "ByDeviceName";
            DeviceNameParam.AllowMultiple = true;
            DeviceNameParam.Position = 1;
            return DeviceNameParam.GetParamDictionary();
        }

        public RemoveCylanceDevice()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_REMOVE_DEVICE, "Removing device(s)");
            EnableOutput = false;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ObjectsPerTransaction = MAX_UPDATES_PER_TRANSACTION;

            switch (ParameterSetName)
            {
                case "ByDeviceName":
                    var devices = Api.Devices;
                    var values = DeviceNameParam.Values;
                    foreach (var value in values)
                    {
                        var id = (from d in devices where d.device_name.Equals(value, StringComparison.InvariantCultureIgnoreCase) select d.device_id).First();
                        QueueObject(id);
                    }
                    break;
            }
        }

        protected override Task[] CreateTasksFromObjects(Guid[] q)
        {
            return new [] { Connection.RemoveDevicesAsync(q.ToArray()) };
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            QueueObject(Device.device_id);
        }
    }
}
