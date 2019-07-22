using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionRuleSet
{
    [Cmdlet(VerbsCommon.Get, "CylanceDetectionRuleSet")]
    [OutputType(typeof(CyDetectionRuleSet))]
    public class GetCylanceDetectionRuleSet : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public CyDetectionRuleSetMetaData RuleSet { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (RuleSet != null)
                WriteObject(Connection.GetDetectionRuleSetAsync(RuleSet).WaitAndUnwrapException());
        }

    }
}
