using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Select
{
    [Cmdlet(VerbsCommon.Select, "CylanceDetection")]
    [OutputType(typeof(CyDetectionMetaData))]
    public class SelectCylanceDetection : CyApiCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyDetectionMetaData Detection { get; set; }

        protected PowershellDynamicParameter PhoneticIdParam;
        protected PowershellDynamicParameter DeviceNameParam;
        protected PowershellDynamicParameter RuleNameParam;

        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;

            PhoneticIdParam = new PowershellDynamicParameter(this, "PhoneticId", (from d in Api.Detections select d.PhoneticId).ToArray());
            PhoneticIdParam.AllowMultiple = true;

            DeviceNameParam = new PowershellDynamicParameter(this, "Device", (from d in Api.Devices select d.device_name).ToArray());
            DeviceNameParam.AllowMultiple = true;

            RuleNameParam = new PowershellDynamicParameter(this, "Rule", (from d in Api.DetectionRules select d.Name).ToArray());
            RuleNameParam.AllowMultiple = true;

            var dynamicParams = PhoneticIdParam.GetParamDictionary();
            DeviceNameParam.AddToParamDictionary(dynamicParams);
            RuleNameParam.AddToParamDictionary(dynamicParams);
            return dynamicParams;
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            if ((Detection != null) && Detection.MatchesFilter(PhoneticIdParam.Values, DeviceNameParam.Values, RuleNameParam.Values))
                WriteObject(Detection);
        }
    }
}
