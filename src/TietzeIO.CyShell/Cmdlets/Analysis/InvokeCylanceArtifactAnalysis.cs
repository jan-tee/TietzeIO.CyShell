using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Analytics;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    /// <summary>
    /// Performs an analysis of cached (full) detection records.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "CylanceArtifactAnalysis", SupportsShouldProcess = true)]
    [OutputType(typeof(DetectionAnalyzer.DetectionRuleStats))]
    public class InvokeCylanceArtifactAnalysis : CyCachedModeCmdlet
    {
        [Parameter(Mandatory = false)]
        public SwitchParameter CreateExceptions { get; set; }

        [Parameter]
        public SwitchParameter PerVersionStats { get; set; }

        //private PowershellDynamicParameter _ruleNameParam;
        //private PowershellDynamicParameter _deviceNameParam;

        //public object GetDynamicParameters()
        //{
        //    if (!API.CacheMode) return null;

        //    _deviceNameParam =
        //        new PowershellDynamicParameter(this, "Device", (from d in API.Devices select d.device_name).ToArray())
        //        {
        //            AllowMultiple = true
        //        };

        //    _ruleNameParam = new PowershellDynamicParameter(this, "Rule", (from d in API.DetectionRules select d.Name).ToArray())
        //    {
        //        AllowMultiple = true
        //    };

        //    var dynamicParams = _deviceNameParam.GetParamDictionary();
        //    _ruleNameParam.AddToParamDictionary(dynamicParams);
        //    return dynamicParams;
        //}

        public InvokeCylanceArtifactAnalysis()
        {
            RequireProtectCache = false;
            RequireOpticsCache = true;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            var detections = new DetectionFilter().Filter<CyDetection>(Api.DetectionsDetail).
                        Where(d => d.DetectionRule != null);

            var config = ConfigurationData.Default.DetectionsAnalyzerRules.DetectionGroupingRules.Values;

            var da = new DetectionAnalyzer(config.ToList())
            {
                KeepPerVersionStats = PerVersionStats.IsPresent
            };
            da.AnalyzeDetectionsByArtifacts(
                detections);

            WriteObject(da.RuleStats.OrderByDescending(rs => rs.NumberOfDetections), true);
        }

    }
}