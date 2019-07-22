using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionRuleSet
{
    [Cmdlet(VerbsCommon.Get, "CylanceDetectionRuleSets")]
    [OutputType(typeof(CyDetectionRuleSetMetaData))]
    public class GetCylanceDetectionRuleSets : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.GetDetectionRuleSetsAsync().
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }
    }
}
