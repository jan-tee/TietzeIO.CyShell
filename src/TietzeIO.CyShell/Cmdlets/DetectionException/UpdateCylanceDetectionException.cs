using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionException
{
    [Cmdlet(VerbsData.Update, "CylanceDetectionException")]
    [OutputType(typeof(CyDetectionException))]
    public class UpdateCylanceDetectionException : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public CyDetectionException DetectionException { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (DetectionException != null)
            {
                WriteObject(Connection.UpdateDetectionExceptionAsync(DetectionException).WaitAndUnwrapException());
            }
        }
    }
}
