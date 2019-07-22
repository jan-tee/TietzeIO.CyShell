using Nito.AsyncEx.Synchronous;
using System.Management.Automation;
using TietzeIO.CyAPI.CAE;
using TietzeIO.CyAPI.CAE.RuleElements;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionRule
{
    [Cmdlet(VerbsCommon.Get, "CylanceDetectionRule")]
    [OutputType(typeof(Rule))]
    public class GetCylanceDetectionRule : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public CyDetectionRuleMetaData Rule { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (Rule != null)
                WriteObject(Connection.GetDetectionRuleAsync(Rule).WaitAndUnwrapException());
        }
    }
}
