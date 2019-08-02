using Nito.AsyncEx.Synchronous;
using System.Management.Automation;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Policy
{
    [Cmdlet(VerbsData.Update, "CylancePolicy")]
    public class UpdateCylancePolicy : CyApiCmdlet
    {
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
        public CyPolicy Policy { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            Connection
                .UpdatePolicyAsync(Policy)
                .WaitAndUnwrapException();
        }
    }
}
