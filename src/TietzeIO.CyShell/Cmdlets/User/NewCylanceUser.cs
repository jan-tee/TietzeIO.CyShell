using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.User
{
    [Cmdlet(VerbsCommon.New, "CylanceUser")]
    [OutputType(typeof(CyUser))]
    public class NewCylanceUser : CyApiCmdlet
    {
        [Parameter(Mandatory = true)]
        public string FirstName { get; set; }
        [Parameter(Mandatory = true)]
        public string LastName { get; set; }
        [Parameter(Mandatory = true)]
        public string Email { get; set; }
        [Parameter(Mandatory = true)]
        [ValidateSet("Administrator", "User", "ZoneManager")]
        public string Role { get; set; }
        [Parameter(Mandatory = false)]
        public string ZoneRights { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

        }   
    }
}
