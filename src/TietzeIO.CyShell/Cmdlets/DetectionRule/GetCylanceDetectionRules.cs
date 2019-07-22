using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionRule
{
    [Cmdlet(VerbsCommon.Get, "CylanceDetectionRules")]
    [OutputType(typeof(CyDetectionRuleMetaData))]
    public class GetCylanceDetectionRules : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                GetDetectionRulesAsync().
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }
    }
}
