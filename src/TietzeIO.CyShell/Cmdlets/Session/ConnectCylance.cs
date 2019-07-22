using System.Linq;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Session;
using static TietzeIO.CyAPI.ApiV2;

namespace TietzeIO.CyShell.Cmdlets.Session
{
    [Cmdlet(VerbsCommunications.Connect, "Cylance")]
    [OutputType(typeof(ApiConnectionHandle))]
    public class ConnectCylance : CyCmdlet, IDynamicParameters
    {
        [Parameter]
        [ValidateSet("Session", "None")]
        public string Scope { get; set; }

        private bool _protectCache;
        [Parameter]
        public SwitchParameter ProtectCache { get => _protectCache; set { _protectCache = value; } }

        private bool _opticsCache;
        [Parameter]
        public SwitchParameter OpticsCache { get => _opticsCache; set { _opticsCache = value; } }

        [Parameter(Mandatory = false)]
        public string ProxyServer { get; set; }

        private PowershellDynamicParameter _consolesParam;
        public object GetDynamicParameters()
        {
            // return new ConsoleDynamicParameter();
            _consolesParam = new PowershellDynamicParameter(this, "Console", (from c in ConfigurationManager.Default.All() orderby c.ConsoleId select c.ConsoleId).ToArray());
            _consolesParam.Position = 1;
            return _consolesParam.GetParamDictionary();
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            var console = ConfigurationManager.Default.Get(_consolesParam.Value);
            WriteVerbose($"Found console with API ID: {console.APIId}");

            var apiSession = new ApiV2Session
            {
                APIId = console.APIId,
                APISecret = console.APISecret.Value,
                APITenantId = console.APITenantId,
                APIBaseUrl = console.APIUrl,
                Proxy = (string.IsNullOrEmpty(ProxyServer)) ? null : new System.Net.WebProxy(ProxyServer),
            };

            // create session object
            var session = new ApiConnectionHandle(apiSession)
            {
                IncludeProtectData = _protectCache,
                IncludeOpticsData = _opticsCache
            };

            // log to this Powershell cmdlet's context
            ApiV2 connection = new ApiV2(apiSession);

            connection.ConnectAsync().WaitAndUnwrapException();

            if (ProtectCache || OpticsCache)
            {
                session.RefreshCache(connection, this);
                session.CacheMode = true;
            }
            else
            {
                session.ClearCache(this);
            }

            if ("Session".Equals(Scope) || string.IsNullOrEmpty(Scope))
            {
                // default to session scope
                session.MakeGlobal();
            }
            else
            {
                WriteObject(session);
            }
        }
    }
}
