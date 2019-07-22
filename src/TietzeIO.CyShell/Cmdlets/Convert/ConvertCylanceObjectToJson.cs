using System.Management.Automation;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.JSON;
using TietzeIO.CyShell.Session;

namespace TietzeIO.CyShell.Cmdlets.Convert
{
    [Cmdlet(VerbsData.Convert, "CylanceObjectToJson")]
    [OutputType(typeof(ApiConnectionHandle))]
    public class GetCylanceSession : Cmdlet
    {
        [Parameter(Mandatory =true, ValueFromPipeline =true)]
        public ICyEntity Object { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            WriteObject(
                ApiV2JsonSerializer.Default.Serialize(Object));
        }
    }
}
