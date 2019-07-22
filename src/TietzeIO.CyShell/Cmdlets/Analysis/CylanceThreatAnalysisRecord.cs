using System.Collections.Generic;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    public partial class InvokeCylanceThreatAnalysis
    {
        public class CylanceThreatAnalysisRecord
        {

            public long Count { get => Threats.Count; }
            public string Classification { get; set; }
            public List<CyThreatMetaData> Threats { get; set; }
            public CylanceThreatAnalysisRecord(string classification, IEnumerable<CyThreatMetaData> threats)
            {
                Threats = new List<CyThreatMetaData>();
                Classification = classification;
                Threats.AddRange(threats);
            }
        }

    }
}
