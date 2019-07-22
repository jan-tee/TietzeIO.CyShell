using System;
using System.Linq;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Policy
{
    [Cmdlet(VerbsCommon.Remove, "CylancePolicy")]
    [OutputType(typeof(CyPolicy))]
    public class RemoveCylancePolicy : CyApiCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByPolicyMetaData")]
        public CyPolicyMinimalMetaData Policy { get; set; }

        private PowershellDynamicParameter PoliciesParam;
        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;
            PoliciesParam = new PowershellDynamicParameter(this, "Name", (from p in Api.Policies select p.name).ToArray());
            PoliciesParam.ParameterSetName = "ByPolicyName";
            PoliciesParam.AllowMultiple = true;
            PoliciesParam.Position = 1;
            return PoliciesParam.GetParamDictionary();
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            switch (ParameterSetName)
            {
                case "ByPolicyName":
                    var policies = Api.Policies;
                    var values = PoliciesParam.Values;
                    foreach (var value in values)
                    {
                        var id = (from p in policies where p.name.Equals(value, StringComparison.InvariantCultureIgnoreCase) select p.id).First();
                        if (id.HasValue)
                        {
                            Connection.DeletePolicyAsync(id.Value).WaitAndUnwrapException();
                        }
                    }
                    break;
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            switch (ParameterSetName)
            {
                case "ByPolicyMetaData":
                    if ((Policy != null) && (Policy.id.HasValue))
                        Connection.DeletePolicyAsync(Policy.id.Value).WaitAndUnwrapException();
                    break;
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}
