using System;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Threading.Tasks;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Lockdown
{
    [Cmdlet(VerbsCommon.Get, "CylanceLockdownHistory")]
    [OutputType(typeof(CyRestLockdownHistory))]
    public class GetCylanceLockdownHistory : CyParallelPipelineCmdlet<CyRestLockdownHistory, CyDeviceMinimalMetaData>, IDynamicParameters
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true)]
        public CyDeviceMinimalMetaData Device { get; set; }

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

        public GetCylanceLockdownHistory()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GET_LOCKDOWN_STATUS, "Fetch lockdown status");
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
                        var lockdownHistory = Connection.RequestLockdownHistoryAsync(id).WaitAndUnwrapException();
                        if (null != lockdownHistory)
                        {
                            WriteObject(lockdownHistory);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            switch (ParameterSetName)
            {
                case "ByDeviceName":
                    break;
                default:
                    QueueObject(Device);
                    break;
            }
        }

        protected override Task[] CreateTasksFromObjects(CyDeviceMinimalMetaData[] q)
        {
            //return new Task[] { Connection.RequestLockdownHistoryAsync(q[0]) };

            return new[] {
                AttemptAndSoftFail(
                    Connection.RequestLockdownHistoryAsync(q[0]),
                    new [] { $"Not found: {q[0].device_name} ({q[0].device_id})" },
                    new [] { HttpStatusCode.NotFound })
                };
        }
    }
}
