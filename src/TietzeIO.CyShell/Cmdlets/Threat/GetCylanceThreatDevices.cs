using System;
using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Threat
{
    [Cmdlet(VerbsCommon.Get, "CylanceThreatDevices")]
    [OutputType(typeof(CyThreatDevice))]
    public class GetCylanceThreatDevices : CyParallelPipelineCmdlet<object, object>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByThreat")]
        public PSObject Threat { get; set; }

        public GetCylanceThreatDevices() : base()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_GET_THREATDEVICES,
                "Retrieving ThreatDevices (threat instances)");
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if ((Threat.BaseObject as string) != null)
            {
                var sha256 = Threat.BaseObject as string;
                QueueTask(Retrieve(sha256));
            }
            else if ((Threat.ImmediateBaseObject as string) != null)
            {
                var sha256 = Threat.ImmediateBaseObject as string;
                QueueTask(Retrieve(sha256));
            }
            else if ((Threat.BaseObject as CyThreatMetaData) != null)
            {
                var metadata = Threat.BaseObject as CyThreatMetaData;
                QueueTask(Retrieve(metadata));
            }
            else if ((Threat.BaseObject as ICyThreat) != null)
            {
                var sha256 = (Threat.BaseObject as ICyThreat).sha256;
                QueueTask(Retrieve(sha256));
            }
            else
            {
                throw new Exception($"Threat must be a SHA256 string, or an instance of ICyThreat, or a type derived from CyThreatMetaData, but it is {Threat.BaseObject.GetType()}");
            }
        }

        private async Task<object> Retrieve(string sha256)
        {
            return await Connection.RequestThreatDevicesAsync(sha256);
        }
        private async Task<object> Retrieve(CyThreatMetaData metadata)
        {
            return await Connection.RequestThreatDevicesAsync(metadata);
        }
    }
}

