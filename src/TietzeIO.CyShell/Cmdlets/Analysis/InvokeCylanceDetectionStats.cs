using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    [Cmdlet(VerbsLifecycle.Invoke, "CylanceDetectionStats")]
    [OutputType(typeof(object))]
    public partial class InvokeCylanceDetectionStats : CyCachedModeCmdlet
    {
        [Parameter]
        public SwitchParameter IncludeNoNewHitRules { get; set; }

        [Parameter(Mandatory = false)]
            [ValidateSet("Total", "Name", "New")]
            public string OrderBy { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet("All Devices", "Per Device")]
        public string Scope { get; set; }

        public InvokeCylanceDetectionStats()
        {
            RequireProtectCache = true;
            RequireOpticsCache = true;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            switch (Scope)
            {
                case "Per Device":
                    {
                        var outputRecords = new List<DetectionAnalysisDeviceRecord>();
                        var deviceGuids = (from d in Api.Detections where d != null select d.Device.CylanceId).Distinct();
                        foreach (var deviceGuid in deviceGuids)
                        {
                            var device = (from d in Api.Devices where deviceGuid.Equals(d.device_id) select d).First();
                            WriteVerbose($"Processing device: {device.device_name}");
                            var detections = from d in Api.Detections
                                             where ((d != null) && (d.Device != null) && (d.Device.CylanceId != null) && d.Device.CylanceId.Equals(deviceGuid))
                                             orderby d.Device.name
                                             select d;
                            var output = Analyze(detections, IncludeNoNewHitRules.IsPresent);
                            var dar = new DetectionAnalysisDeviceRecord();
                            dar.Device = device;
                            dar.PerRuleStats.AddRange(output);
                            outputRecords.Add(dar);
                        }
                        if (outputRecords.Count > 0)
                        {
                            switch (OrderBy)
                            {
                                case "Name":
                                    WriteObject((from r in outputRecords orderby r.Device.device_name ascending, r.New descending select r), true);
                                    break;
                                case "New":
                                    WriteObject((from r in outputRecords orderby r.New descending select r), true);
                                    break;
                                case "Total":
                                default:
                                    WriteObject((from r in outputRecords orderby r.Total descending select r), true);
                                    break;
                            }
                        }
                    }
                    break;
                case "All Devices":
                default:
                    {
                        var outputRecords = new List<DetectionAnalysisRecord>();

                        var detections = from d in Api.Detections
                                         where d != null
                                         select d;
                        outputRecords.AddRange(Analyze(detections, IncludeNoNewHitRules.IsPresent));
                        if (outputRecords.Count > 0)
                        {
                            switch (OrderBy)
                            {
                                case "Name":
                                    WriteObject((from r in outputRecords orderby r.Rule.Name select r), true);
                                    break;
                                case "Total":
                                    WriteObject((from r in outputRecords orderby r.Total descending select r), true);
                                    break;
                                case "New":
                                default:
                                    WriteObject((from r in outputRecords orderby r.New descending select r), true);
                                    break;
                            }
                        }
                    }
                    break;
            }


        }

        private List<DetectionAnalysisRecord> Analyze(IEnumerable<CyDetectionMetaData> detections, bool includeRulesWithNoNewHits = false)
        {
            var output = new List<DetectionAnalysisRecord>();
            foreach (var rule in Api.DetectionRules)
            {
                var dar = new DetectionAnalysisRecord();
                dar.Rule = rule;

                foreach (var detection in
                    (from d in detections where rule.Name.Equals(d.DetectionRuleName) group d by d.Status into g select new { Status = g.Key, Detections = g.ToList() }))
                {
                    switch (detection.Status)
                    {
                        case "New":
                            dar.New = detection.Detections.Count();
                            break;
                        case "InProgress":
                            dar.InProgress = detection.Detections.Count();
                            break;
                        case "Reviewed":
                            dar.Reviewed = detection.Detections.Count();
                            break;
                        case "False Positive":
                            dar.FalsePositive = detection.Detections.Count();
                            break;
                        case "Follow Up":
                            dar.FollowUp = detection.Detections.Count();
                            break;
                        case "Done":
                            dar.Done = detection.Detections.Count();
                            break;
                    }
                }
                if ((dar.New > 0) || includeRulesWithNoNewHits)
                {
                    output.Add(dar);
                }
            }
            return output;
        }
    }
}