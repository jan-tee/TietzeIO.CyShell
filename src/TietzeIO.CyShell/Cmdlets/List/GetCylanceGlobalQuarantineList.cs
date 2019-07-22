using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.List
{
    [Cmdlet(VerbsCommon.Get, "CylanceGlobalQuarantineList")]
    [OutputType(typeof(CyGlobalList))]
    public class GetCylanceGlobalQuarantineList : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            var l = Connection.
                RequestGlobalQuarantineListAsync().
                WaitAndUnwrapException();

            if (l != null)
                l.ForEach(WriteObject);
        }
    }
}
