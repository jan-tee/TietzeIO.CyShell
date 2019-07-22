using System;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Detection
{
    [Cmdlet(VerbsCommon.Get, "CylanceDetections")]
    [OutputType(typeof(CyDeviceMetaData))]

    public class GetCylanceDetections : CyApiCmdlet
    {
        [Parameter]
        public DateTime? Start { get; set; }
        [Parameter]
        public DateTime? End { get; set; }
        [Parameter]
        [ValidateSet("New", "False Positive", "Follow Up", "In Progress", "Reviewed", "Done")]
        public string Status { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                GetDetectionsAsync(Start, End, Status).
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }
    }
}
