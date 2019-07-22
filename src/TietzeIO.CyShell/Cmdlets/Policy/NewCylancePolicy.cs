using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Policy
{
    [Cmdlet(VerbsCommon.New, "CylancePolicy")]
    [OutputType(typeof(CyPolicy))]
    public class NewCylancePolicy : CyApiCmdlet
    {
        [Parameter(Mandatory = true, Position = 1)]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            CyPolicy policy = new CyPolicy(Name);
            policy = Connection.CreatePolicyAsync(policy).WaitAndUnwrapException();
            if (null != policy)
            {
                WriteObject(policy);
            }
        }
    }
}
