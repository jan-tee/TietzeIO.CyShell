using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Session;

namespace TietzeIO.CyShell.Cmdlets.Console
{
    [Cmdlet(VerbsCommon.New, "CylanceConsole")]
    public class NewCylanceConsole : CyCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Console { get; set; }

        [Parameter(Mandatory = true)]
        public string APIId { get; set; }

        [Parameter(Mandatory = true)]
        public PSObject APISecret { get; set; }

        [Parameter(Mandatory = true)]
        public string APITenantId { get; set; }

        [Parameter(Mandatory = false)]
        public string TDRToken { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateSet("apne1", "au", "euc1", "sae1", "us-gov", "us")]
        public string Region { get; set; }

        /// <summary>
        /// Login user for a console account
        /// </summary>
        [Parameter(Mandatory = false)]
        public string LoginUser { get; set; }

        /// <summary>
        /// Login password for a console account
        /// </summary>
        [Parameter(Mandatory = false)]
        public PSObject LoginPassword { get; set; }

        /// <summary>
        /// Proxy to use.
        /// </summary>
        [Parameter(Mandatory = false)]
        public string ProxyServer { get; set; }

        /// <summary>
        /// Do not verify credentials before writing into the credentials store.
        /// </summary>
        [Parameter(Mandatory = false)]
        public SwitchParameter NoVerify { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            var TDRUrl = "https://protect.cylance.com/Reports/ThreatDataReportV1/";
            var APIAuthUrl = "https://protectapi.cylance.com/auth/v2/token";
            switch (Region)
            {
                case "apne1":
                case "au":
                case "euc1":
                case "sae1":
                    TDRUrl = $"https://protect-{Region}.cylance.com/Reports/ThreatDataReportV1/";
                    APIAuthUrl = $"https://protectapi-{Region}.cylance.com/auth/v2/token";
                    break;
                case "us-gov":
                    TDRUrl = "https://protect.us.cylance.com/Reports/ThreatDataReportV1/";
                    APIAuthUrl = "https://protectapi.us.cylance.com/auth/v2/token";
                    break;
            }

            var apiSecret = ConvertToSecureString("APISecret", APISecret);

            var console = ConfigurationManager.Default.New();
            console.ConsoleId = Console;
            console.APIId = APIId;
            console.SetAPISecret(apiSecret);
            console.APITenantId = APITenantId;
            console.APIUrl = APIAuthUrl;
            console.TDRUrl = TDRUrl;

            var apiSession = new ApiV2Session
            {
                APIId = console.APIId,
                APISecret = console.APISecret.Value,
                APITenantId = console.APITenantId,
                APIBaseUrl = console.APIUrl,
                Proxy = (string.IsNullOrEmpty(ProxyServer)) ? null : new System.Net.WebProxy(ProxyServer)
            };

            ApiConnectionHandle session = new ApiConnectionHandle(apiSession);
            ApiV2 Connection = new ApiV2(apiSession);
            Connection.ConnectAsync().Wait();

            if (null != LoginPassword)
            {
                var loginPassword = ConvertToSecureString("LoginPassword", LoginPassword);
                console.SetLoginPassword(loginPassword);
            }

            if (!string.IsNullOrEmpty(LoginUser))
            {
                console.LoginUser = LoginUser;
            }

            WriteVerbose($"Writing new console entry '{console.ConsoleId}' for API ID {console.APIId}, Tenant ID {console.APITenantId}, API auth URL {console.APIUrl}, TDR URL {console.TDRUrl}");

            ConfigurationManager.Default.Write(console);
        }


    }
}