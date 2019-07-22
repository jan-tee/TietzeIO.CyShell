using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionException
{
    [Cmdlet(VerbsCommon.New, "CylanceDetectionException")]
    [OutputType(typeof(CyDetectionException))]
    public class NewCylanceDetectionException : CyApiCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            var ex = CyDetectionException.GetNewExceptionTemplate(Name);
            Connection.NewDetectionExceptionAsync(ex).WaitAndUnwrapException();
        }
    }
}
