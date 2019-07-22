using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Policy
{
    [Cmdlet(VerbsData.Update, "CylancePolicy")]
    [OutputType(typeof(CyPolicy))]
    public class UpdateCylancePolicy : CyApiCmdlet
    {
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
        public CyPolicy Policy { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            var policy = Connection.UpdatePolicyAsync(Policy).WaitAndUnwrapException();
            if (null != policy)
            {
                WriteObject(policy);
            }

        }
    }
}
