using System;
using System.Linq;
using System.Management.Automation;
using System.Security;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Console
{
    [Cmdlet(VerbsCommon.Set, "CylanceConsole")]
    public class SetCylanceConsole : CyCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = false)]
        public string APIId { get; set; }

        [Parameter(Mandatory = false)]
        public PSObject APISecret { get; set; }

        [Parameter(Mandatory = false)]
        public string APITenantId { get; set; }

        [Parameter(Mandatory = false)]
        public string TDRToken { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet("apne1", "au", "euc1", "sae1", "us-gov", "us")]
        public string Region { get; set; }

        [Parameter(Mandatory = false)]
        public string LoginUser { get; set; }
        [Parameter(Mandatory = false)]
        public PSObject LoginPassword { get; set; }

        private PowershellDynamicParameter ConsolesParam;
        public object GetDynamicParameters()
        {
            // return new ConsoleDynamicParameter();
            ConsolesParam = new PowershellDynamicParameter(this, "Console",
                (from c in ConfigurationManager.Default.All() orderby c.ConsoleId select c.ConsoleId).ToArray());
            ConsolesParam.Mandatory = true;
            ConsolesParam.Position = 1;
            return ConsolesParam.GetParamDictionary();
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            var console = ConfigurationManager.Default.Get(ConsolesParam.Value);

            var changed = false;
            if (!string.IsNullOrEmpty(LoginUser))
            {
                console.LoginUser = LoginUser;
                changed = true;
            }
            if (null != LoginPassword)
            {
                var p = ConvertToSecureString("LoginPassword", LoginPassword);
                console.SetLoginPassword(p);
                changed = true;
            }
            if (!string.IsNullOrEmpty(TDRToken))
            {
                console.Token = TDRToken;
                changed = true;
            }
            if (!string.IsNullOrEmpty(APIId))
            {
                console.APIId = APIId;
                changed = true;
            }
            if (!string.IsNullOrEmpty(APITenantId))
            {
                console.APITenantId = APITenantId;
                changed = true;
            }
            if (null != APISecret)
            {
                var secret = ConvertToSecureString("APISecret", APISecret);
                console.SetAPISecret(secret);
                changed = true;
            }

            if (changed)
            {
                WriteVerbose("Writing changes");
                ConfigurationManager.Default.Write(console);
            }
            else
            {
                WriteVerbose("No changes to process.");
            }
        }
    }
}