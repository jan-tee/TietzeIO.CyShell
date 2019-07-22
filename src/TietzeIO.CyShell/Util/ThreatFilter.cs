using TietzeIO.CyAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using TietzeIO.CyAPI.Entities;

namespace TietzeIO.CyShell.Util
{
    public class ThreatFilter
    {
        public static CyThreatMetaData GetThreat(List<CyThreatMetaData> threats, string sha256)
        {
            return (from t in threats where t.sha256.Equals(sha256, StringComparison.InvariantCultureIgnoreCase) select t).First();
        }

        public static bool FilterThreatDevice(List<CyThreatMetaData> threats, CyThreatDevice threatDevice, string[] classificationFilter, string[] subClassificationFilter, string[] nameFilter, string[] literalPathFilter)
        {
            var threat = GetThreat(threats, threatDevice.sha256);

            bool includeInResults = FilterThreat(threat, classificationFilter, subClassificationFilter, nameFilter);

            if ((includeInResults) && (null != literalPathFilter))
            {
                var match = false;
                foreach (var path in literalPathFilter)
                {
                    if (threatDevice.file_path.StartsWith(path, StringComparison.InvariantCultureIgnoreCase))
                    {
                        match = true;
                    }
                }
                includeInResults &= match;
            }
            return includeInResults;
        }

        public static bool FilterThreat(CyThreatMetaData threat, string[] classificationFilter, string[] subClassificationFilter, string[] nameFilter = null,
            string[] literalPathFilter = null,
            string[] globPathFilter = null,
            string[] regexPathFilter = null)
        {
            bool classificationFilterHasWildcard = false;
            if (classificationFilter != null)
            {
                classificationFilterHasWildcard = classificationFilter.Contains("*");
            }
            bool sub_classificationFilterHasWildcard = false;
            if (subClassificationFilter != null)
            {
                sub_classificationFilterHasWildcard = subClassificationFilter.Contains("*");
            }

            bool includeInResults = true;
            if ((null != classificationFilter) && (!classificationFilterHasWildcard))
            {
                includeInResults &= classificationFilter.Contains(threat.classification, StringComparer.InvariantCultureIgnoreCase);
            }
            if ((includeInResults) && ((null != subClassificationFilter) && (!sub_classificationFilterHasWildcard)))
            {
                includeInResults &= subClassificationFilter.Contains(threat.sub_classification, StringComparer.InvariantCultureIgnoreCase);
            }
            if ((includeInResults) && (null != nameFilter))
            {
                includeInResults &= nameFilter.Contains(threat.name, StringComparer.InvariantCultureIgnoreCase);
            }
            return includeInResults;
        }

        public static List<T> FilterThreats<T>(List<T> threats, string[] classificationFilter, string[] subClassificationFilter, string[] nameFilter = null) where T : CyThreatMetaData
        {
            var results = new List<T>();
            foreach (var threat in threats)
            {
                if (FilterThreat(threat, classificationFilter, subClassificationFilter, nameFilter))
                {
                    results.Add(threat);
                }
            }

            return results;
        }
    }
}