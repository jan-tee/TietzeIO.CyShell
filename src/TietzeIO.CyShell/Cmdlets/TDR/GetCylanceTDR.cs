using System;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyAPI.Configuration.DPAPI;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyAPI.TDR;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.TDR
{
    [Cmdlet(VerbsCommon.Get, "CylanceTDR")]
    [OutputType(typeof(CyDetection))]
    public class GetCylanceTDR : CyParallelPipelineCmdlet<object, IConsoleConfig>, IDynamicParameters
    {
        private PowershellDynamicParameter _consolesParam;
        public object GetDynamicParameters()
        {
            _consolesParam = new PowershellDynamicParameter(this, "Console", (from c in CyAPI.Configuration.DPAPI.ConfigurationManager.Default.All() orderby c.ConsoleId select c.ConsoleId).ToArray());
            _consolesParam.AllowMultiple = true;
            _consolesParam.Position = 1;
            return _consolesParam.GetParamDictionary();
        }

        private DateTime timestamp = DateTime.Now;

        public GetCylanceTDR()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_DOWNLOAD_TDR, "Downloading TDRs");
        }

        protected override Task[] CreateTasksFromObjects(IConsoleConfig[] q)
        {
            var downloader = new ThreatDataCsvDownloader(q[0], timestamp);
            return new[] { downloader.DownloadAsync() };
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            foreach (var v in _consolesParam.Values)
            {
                var console = CyAPI.Configuration.DPAPI.ConfigurationManager.Default.Get(v);
                QueueObject(console);
            }
        }
    }
}
