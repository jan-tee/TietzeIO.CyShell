using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Detection.Base
{
    public abstract class CyDetectionsMassOperationsCmdlet<T> : CyParallelPipelineCmdlet<T, ICyDetection>, IDynamicParameters where T : new()
    {
        protected PowershellDynamicParameter PhoneticIdParam;
        protected PowershellDynamicParameter DeviceNameParam;
        protected PowershellDynamicParameter RuleNameParam;

        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;

            PhoneticIdParam = new PowershellDynamicParameter(this, "PhoneticId", (from d in Api.Detections select d.PhoneticId).ToArray());
            //PhoneticIdParam.ParameterSetName = "ByPhoneticId";
            PhoneticIdParam.AllowMultiple = true;

            DeviceNameParam = new PowershellDynamicParameter(this, "Device", (from d in Api.Devices select d.device_name).ToArray());
            //DeviceNameParam.ParameterSetName = "ByDevice";
            DeviceNameParam.AllowMultiple = true;

            RuleNameParam = new PowershellDynamicParameter(this, "Rule", (from d in Api.Detections select d.DetectionRuleName).Distinct().OrderBy(d => d).ToArray());
            //RuleNameParam.ParameterSetName = "ByRule";
            RuleNameParam.AllowMultiple = true;

            var dynamicParams = PhoneticIdParam.GetParamDictionary();
            DeviceNameParam.AddToParamDictionary(dynamicParams);
            RuleNameParam.AddToParamDictionary(dynamicParams);
            return dynamicParams;
        }
    }
}
