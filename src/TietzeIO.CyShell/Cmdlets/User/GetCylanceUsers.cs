using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.User
{
    [Cmdlet(VerbsCommon.Get, "CylanceUsers")]
    [OutputType(typeof(CyUser))]
    public class GetCylanceUsers : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                RequestUserListAsync().
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }
    }
}
