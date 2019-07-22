using System.Management.Automation;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Session;

namespace TietzeIO.CyShell.Cmdlets.Session
{
    [Cmdlet(VerbsCommon.Set, "CylanceSession")]
    [OutputType(typeof(ApiConnectionHandle))]
    public class SetCylanceSession : CyApiCmdlet
    {
        [Parameter]
        public bool? Cache { get; set; }

        [Parameter]
        public string Proxy { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            if (Proxy != null)
            {
                WriteVerbose($"Setting proxy server: {Proxy}");
                Api.Session.Proxy = new System.Net.WebProxy(Proxy);
            }

            if (Cache.HasValue)
            {
                if (Cache.Value)
                {
                    // hyper comfort mode
                    WriteVerbose("Initializing cache.");
                    Api.RefreshCache(Connection, this);
                    Api.CacheMode = true;
                }
                else
                {
                    Api.CacheMode = false;
                    Api.ClearCache(this);
                }
            }
        }
    }
}
