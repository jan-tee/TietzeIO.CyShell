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
    [Cmdlet(VerbsCommon.Copy, "CylancePolicy")]
    [OutputType(typeof(CyPolicy))]
    public class CopyCylancePolicy : CyApiCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, ParameterSetName = "ByPolicy")]
        public CyPolicy Source { get; set; }

        [Parameter(Mandatory = true)]
        public string TargetName { get; set; }

        private PowershellDynamicParameter SourcePolicyParam;
        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;
            SourcePolicyParam = new PowershellDynamicParameter(this, "SourceName", (from p in Api.Policies select p.name).ToArray());
            SourcePolicyParam.ParameterSetName = "ByPolicyName";
            SourcePolicyParam.AllowMultiple = true;
            SourcePolicyParam.Position = 1;
            return SourcePolicyParam.GetParamDictionary();
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            switch (ParameterSetName)
            {
                case "ByPolicyName":
                    var policies = Api.Policies;
                    var values = SourcePolicyParam.Values;
                    foreach (var value in values)
                    {
                        var id = (from p in policies where p.name.Equals(value, StringComparison.InvariantCultureIgnoreCase) select p.id).First();
                        var policy = Connection.GetPolicyAsync(id.Value).WaitAndUnwrapException();
                        if (null != policy)
                        {
                            CloneAction(policy);
                        }
                        else
                        {
                            throw new Exception("Failed to get source policy.");
                        }
                    }
                    break;
                case "__AllParameterSets":
                case "ByPolicy":
                    CloneAction(Source);
                    break;
            }
        }

        private void CloneAction(CyPolicy source)
        {
            var policy = source.GetClone();
            policy.policy_name = TargetName;
            WriteObject(Connection.CreatePolicyAsync(policy).WaitAndUnwrapException());
        }
    }
}
