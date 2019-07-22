using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.DetectionException
{
    [Cmdlet(VerbsCommon.Get, "CylanceDetectionExceptions")]
    [OutputType(typeof(CyDetectionRuleMetaData))]
    public class GetCylanceDetectionExceptions : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                GetDetectionExceptionsAsync().
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }
    }
}
