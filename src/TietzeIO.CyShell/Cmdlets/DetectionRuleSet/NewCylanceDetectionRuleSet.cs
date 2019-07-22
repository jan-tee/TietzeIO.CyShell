using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionRuleSet
{
    [Cmdlet(VerbsCommon.New, "CylanceDetectionRuleSet")]
    [OutputType(typeof(CyDetectionRuleSet))]
    public class NewCylanceDetectionRuleSet : CyApiCmdlet
    {

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public string Description { get; set; }

        [Parameter(Mandatory = true)]
        public string NotificationMessage{ get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            var r = Connection.NewDetectionRuleSetAsync(Name, Description, NotificationMessage).WaitAndUnwrapException();
            WriteObject(r);
        }
    }
}
