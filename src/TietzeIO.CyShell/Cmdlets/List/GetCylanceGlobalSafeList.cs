using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.List
{
    [Cmdlet(VerbsCommon.Get, "CylanceGlobalSafeList")]
    [OutputType(typeof(CyGlobalList))]
    public class GetCylanceGlobalSafeList : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            var l = Connection.
                RequestGlobalSafeListAsync().
                WaitAndUnwrapException();
            if (l != null)
                l.ForEach(WriteObject);
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }
    }
}
