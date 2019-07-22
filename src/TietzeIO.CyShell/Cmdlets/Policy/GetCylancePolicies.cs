using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Policy
{
    [Cmdlet(VerbsCommon.Get, "CylancePolicies")]
    [OutputType(typeof(CyPolicyMetaData))]
    public class GetCylancePolicies : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                GetPoliciesAsync().
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }
    }
}
