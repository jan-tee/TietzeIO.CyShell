using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionException
{
    [Cmdlet(VerbsCommon.Get, "CylanceDetectionException")]
    [OutputType(typeof(CyDetectionException))]
    public class GetCylanceDetectionException : CyApiCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public CyDetectionExceptionMetaData DetectionException { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (DetectionException != null)
            {
                WriteObject(Connection.GetDetectionExceptionAsync(DetectionException).WaitAndUnwrapException());
            }
        }
    }
}
