using System;
using System.Linq;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.User
{
    [Cmdlet(VerbsCommon.Get, "CylanceUser")]
    [OutputType(typeof(CyUser))]
    public class GetCylanceUser : CyApiCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = false, ValueFromPipeline = true, ParameterSetName = "ByUser")]
        public CyUser User { get; set; }

        private PowershellDynamicParameter EmailParam;

        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;
            EmailParam = new PowershellDynamicParameter(this, "Email", (from u in Api.Users select u.email).ToArray());
            EmailParam.ParameterSetName = "ByEmail";
            EmailParam.AllowMultiple = false;
            EmailParam.Mandatory = false;
            EmailParam.Position = 1;
            return EmailParam.GetParamDictionary();
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            switch (ParameterSetName)
            {
                case "ByEmail":
                    string email = EmailParam.Value;
                    var users = from u in Api.Users where email.Equals(u.email, StringComparison.InvariantCultureIgnoreCase) select u;
                    if (users.Any())
                    {
                        var user = users.First();
                        WriteObject(Connection.RequestUserAsync(user).WaitAndUnwrapException());
                    }
                    break;
                case "__AllParameterSets":
                // pipeline case
                case "ByUser":
                    break;
                default:
                    throw new Exception("Could not determine user identity. Maybe the call is missing a required argument?");
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            switch (ParameterSetName)
            {
                case "ByUser":
                    if (User != null)
                        WriteObject(Connection.RequestUserAsync(User).WaitAndUnwrapException());
                    break;
                case "ByEmail":
                    break;
            }
        }
    }
}
