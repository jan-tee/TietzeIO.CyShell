using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Device
{
    [Cmdlet(VerbsCommon.Get, "CylanceDevice")]
    [OutputType(typeof(CyDeviceMetaData))]
    public class GetCylanceDevice : CyParallelPipelineCmdlet<CyDevice, Guid>, IDynamicParameters
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDevice")]
        public CyDeviceBase Device { get; set; }

        [Parameter(ParameterSetName = "ByMAC")]
        [ValidatePattern(@"[0-9a-f]{2}[:-][0-9a-f]{2}[:-][0-9a-f]{2}[:-][0-9a-f]{2}[:-][0-9a-f]{2}[:-][0-9a-f]{2}")]
        public string MAC { get; set; }

        [Parameter]
        public SwitchParameter WithZones { get; set; }

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

        public GetCylanceDevice()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GET_DEVICE, "Getting device detail");
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            switch (ParameterSetName)
            {
                case "ByDeviceName":
                    var devices = Api.Devices;
                    var values = DeviceNameParam.Values;
                    foreach (var value in values)
                    {
                        var id = (from d in devices where d.device_name.Equals(value, StringComparison.InvariantCultureIgnoreCase) select d.device_id).First();
                        var dv = Connection.RequestDeviceDetailsAsync(id).WaitAndUnwrapException();
                        if (null != dv) WriteObject(dv);
                    }
                    break;
                case "ByMAC":
                    var device = Connection.RequestDeviceDetailsAsync(MAC).WaitAndUnwrapException();
                    if (null != device)
                    {
                        WriteObject(device);
                    }
                    break;
            }
        }

        protected override Task[] CreateTasksFromObjects(Guid[] q)
        {
            switch (ParameterSetName)
            {
                case "ByDevice":
                    // queue a task for each device
                    if (WithZones.IsPresent)
                    {
                        return new[] { Connection.RequestDeviceDetailsWithZonesAsync(q[0]) };
                    }
                    else
                    {
                        return new[] { Connection.RequestDeviceDetailsAsync(q[0]) };
                    }
                    break;
            }
            throw new Exception("Could not determine operation mode.");
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            QueueObject(Device.device_id);
        }
    }
}
