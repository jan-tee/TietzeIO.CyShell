using System;
using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;

namespace TietzeIO.CyShell.Cmdlets.Base
{
    public abstract class CyThreatCmdlet : CyParallelPipelineCmdlet<object, string>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByThreat")]
        public PSObject Threat { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            string sha256;
            if ((Threat.BaseObject as string) != null)
            {
                sha256 = Threat.BaseObject as string;
            }
            else if ((Threat.ImmediateBaseObject as string) != null)
            {
                sha256 = Threat.ImmediateBaseObject as string;
            }
            else if ((Threat.BaseObject as ICyThreat) != null)
            {
                sha256 = (Threat.BaseObject as ICyThreat).sha256;
            }
            else
            {
                throw new Exception($"Threat must be a SHA256 string, or an instance of ICyThreat, but it is {Threat.BaseObject.GetType()}");
            }

            QueueObject(sha256);
        }
    }
}
