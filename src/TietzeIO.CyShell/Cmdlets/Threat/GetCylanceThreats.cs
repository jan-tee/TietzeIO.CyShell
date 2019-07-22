using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Threat
{
    [Cmdlet(VerbsCommon.Get, "CylanceThreats")]
    [OutputType(typeof(CyThreatMetaData))]
    public class GetCylanceThreats : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                RequestThreatListAsync().
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }
    }
}
