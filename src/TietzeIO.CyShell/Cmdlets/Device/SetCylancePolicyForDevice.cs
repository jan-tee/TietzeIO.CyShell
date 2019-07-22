using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Device
{
    [Cmdlet(VerbsCommon.Set, "CylancePolicyForDevice")]
    [OutputType(typeof(CyDeviceMetaData))]
    public class SetCylancePolicyForDevice : CyApiCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDevice")]
        public CyDeviceBase Device { get; set; }

        private Guid policy_id;

        [Parameter(Mandatory = false)]
        public CyPolicyMinimalMetaData Policy { get; set; }

        private PowershellDynamicParameter DeviceNameParam;
        private PowershellDynamicParameter PolicyNameParam;
        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;

            DeviceNameParam = new PowershellDynamicParameter(this, "Name", (from p in Api.Devices select p.device_name).ToArray());
            DeviceNameParam.ParameterSetName = "ByDeviceName";
            DeviceNameParam.AllowMultiple = true;
            DeviceNameParam.Position = 1;
            var d = DeviceNameParam.GetParamDictionary();

            PolicyNameParam = new PowershellDynamicParameter(this, "PolicyName", (from p in Api.Policies select p.name).ToArray());
            PolicyNameParam.Position = 2;
            PolicyNameParam.AddToParamDictionary(d);
            return d;
        }

        private void PerformActionForDevice(CyDeviceBase device, Guid policy_id)
        {
            Connection.SetPolicyForDeviceAsync(device, policy_id).WaitAndUnwrapException();

        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            // determine policy to set first
            if (Policy != null)
            {
                Debug.Assert(Policy.id.HasValue);
                policy_id = Policy.id.Value;
            }
            else
            {
                string policy_name = PolicyNameParam.Value;
                Debug.Assert(policy_name != null);
                policy_id = (from p in Api.Policies where policy_name.Equals(p.name, StringComparison.InvariantCultureIgnoreCase) select p.id.Value).First();
            }

            Debug.Assert(policy_id != null);
            if (policy_id == null)
            {
                throw new Exception("Could not determine policy ID to set.");
            }


            switch (ParameterSetName)
            {
                case "ByDeviceName":
                    // set policy for certain devices

                    var devices = Api.Devices;
                    var values = DeviceNameParam.Values;
                    foreach (var value in values)
                    {
                        var device = (from d in devices where d.device_name.Equals(value, StringComparison.InvariantCultureIgnoreCase) select d).First();
                        PerformActionForDevice(device, policy_id);
                    }
                    break;
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            switch (ParameterSetName)
            {
                case "ByDevice":
                    if (Device != null)
                        PerformActionForDevice(Device, policy_id);
                    break;
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

    }
}
