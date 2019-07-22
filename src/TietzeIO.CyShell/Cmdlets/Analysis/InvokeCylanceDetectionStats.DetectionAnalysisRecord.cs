using TietzeIO.CyAPI.Entities.Optics;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    public partial class InvokeCylanceDetectionStats
    {
        /// <summary>
        /// A result record to be returned
        /// </summary>
        public class DetectionAnalysisRecord
        {
            public CyDetectionRuleMetaData Rule { get; set; }
            public long New { get; set; }
            public long InProgress { get; set; }
            public long FollowUp { get; set; }
            public long Reviewed { get; set; }
            public long FalsePositive { get; set; }
            public long Done { get; set; }
            public long Total { get => (New + InProgress + FollowUp + Reviewed + FalsePositive + Done); }

            public override string ToString()
            {
                return $"{Rule.Name:.20s}: [{Total}]";
            }
        }
    }
}