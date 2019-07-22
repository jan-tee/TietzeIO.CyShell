using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyAPI.TDR.Analytics;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.TDR
{
    [Cmdlet(VerbsData.Convert, "CylanceTDRtoXLSX")]
    [OutputType(typeof(CyDetection))]
    public class ConvertCylanceTDRtoXLSX : CyParallelPipelineCmdlet<object, IConsoleConfig>, IDynamicParameters
    {
        private PowershellDynamicParameter _consolesParam;
        public object GetDynamicParameters()
        {
            _consolesParam = new PowershellDynamicParameter(this, "Console", (from c in ConfigurationManager.Default.All() orderby c.ConsoleId select c.ConsoleId).ToArray());
            _consolesParam.AllowMultiple = true;
            _consolesParam.Position = 1;
            return _consolesParam.GetParamDictionary();
        }

        private DateTime timestamp = DateTime.Now;

        public ConvertCylanceTDRtoXLSX()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_CONVERT_TDR, "Converting TDRs");
            EnableOutput = false;
        }

        private List<ThreatDataAggregator> caches = new List<ThreatDataAggregator>();

        protected override Task[] CreateTasksFromObjects(IConsoleConfig[] items)
        {
            var cache = new ThreatDataAggregator(items[0].ConsoleId);
            caches.Add(cache);
            var jobs = cache.GetConversionTasks();
            return jobs;
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

        protected override void EndProcessing()
        {
            base.EndProcessing();
            WriteObject(caches, true);
        }
    }
}
