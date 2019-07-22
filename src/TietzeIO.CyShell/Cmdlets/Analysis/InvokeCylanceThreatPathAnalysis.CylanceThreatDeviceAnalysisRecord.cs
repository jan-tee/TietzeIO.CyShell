using System;
using System.Collections.Generic;
using System.Linq;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    public partial class InvokeCylanceThreatPathAnalysis
    {
        public class CylanceThreatDeviceAnalysisRecord
        {
            public long Count { get => ThreatDevices.Count; }
            public string Path { get; set; }
            public List<CyThreatDeviceEnriched> ThreatDevices { get; set; }

            /// <summary>
            /// Gets the count of threats with a particular classification
            /// </summary>
            /// <param name="cl"></param>
            /// <returns></returns>
            private long GetCount(string cl)
            {
                var pups = from t in ThreatDevices where cl.Equals(ThreatFilter.GetThreat(CyShell.Session.ApiConnectionHandle.Global.Threats, t.sha256).classification, StringComparison.InvariantCultureIgnoreCase) select t;
                if (!pups.Any())
                    return 0;
                return pups.Count();
            }

            public long PUP { get => GetCount("PUP"); }
            public long Trusted { get => GetCount("Trusted"); }
            public long Malware { get => GetCount("Malware"); }
            public long DualUse { get => GetCount("Dual-Use"); }
            public long Unclassified { get => GetCount(""); }

            public CylanceThreatDeviceAnalysisRecord(string path, IEnumerable<CyThreatDeviceEnriched> threatDevices)
            {
                ThreatDevices = new List<CyThreatDeviceEnriched>();
                Path = path;
                ThreatDevices.AddRange(threatDevices);
            }
        }
    }
}