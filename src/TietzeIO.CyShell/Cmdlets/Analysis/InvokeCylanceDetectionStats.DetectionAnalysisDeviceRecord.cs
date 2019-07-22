using System.Collections.Generic;
using System.Linq;
using TietzeIO.CyAPI.Entities;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    public partial class InvokeCylanceDetectionStats
    {
        public class DetectionAnalysisDeviceRecord
        {
            public CyDeviceMetaData Device { get; set; }
            public long New { get => PerRuleStats.Sum(d => d.New); }
            public long InProgress { get => PerRuleStats.Sum(d => d.InProgress); }
            public long FollowUp { get => PerRuleStats.Sum(d => d.FollowUp); }
            public long Reviewed { get => PerRuleStats.Sum(d => d.Reviewed); }
            public long FalsePositive { get => PerRuleStats.Sum(d => d.FalsePositive); }
            public long Done { get => PerRuleStats.Sum(d => d.Done); }
            public long Total { get => PerRuleStats.Sum(d => d.Total); }
            public List<DetectionAnalysisRecord> PerRuleStats { get; set; }

            public DetectionAnalysisDeviceRecord()
            {
                PerRuleStats = new List<DetectionAnalysisRecord>();
            }
        }
    }
}