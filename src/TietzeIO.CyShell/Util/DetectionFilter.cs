using TietzeIO.CyAPI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;

namespace TietzeIO.CyShell.Util
{
    /// <summary>
    /// Helper methods to filter detections
    /// </summary>
    public class DetectionFilter
    {
        public string[] PhoneticIdFilter { get; set; }
        public string[] DeviceNameFilter { get; set; }
        public string[] RuleNameFilter { get; set; }

        public DetectionFilter()
        {

        }

        /// <summary>
        /// Returns matching detections from the cached list of detections.
        /// </summary>
        /// <returns></returns>
        public List<T> Filter<T>(List<T> haystack) where T : CyDetectionMetaData
        {
            if (haystack == null)
            {
                return new List<T>();
            }

            var detections = from d in haystack where d.MatchesFilter(PhoneticIdFilter, DeviceNameFilter, RuleNameFilter) select d;
            return new List<T>(detections);
        }

        //public static List<T> GetFacet<T>(List<CyDetection.CyArtifact> artifacts, string facetName) where T : class
        //{
        //    List<T> results = new List<T>();
        //    foreach (var artifact in artifacts)
        //    {
        //        switch (facetName)
        //        {
        //            case "Name":
        //                results.Add(artifact.Name as T);
        //                break;
        //            case "Path":
        //                results.Add(artifact.Path as T);
        //                break;
        //            case "CommandLine":
        //                results.Add(artifact.CommandLine as T);
        //                break;
        //            case "Domain":
        //                results.Add(artifact.Domain as T);
        //                break;
        //            case "Sha256Hash":
        //                results.Add(artifact.Sha256Hash as T);
        //                break;
        //            case "Md5Hash":
        //                results.Add(artifact.Md5Hash as T);
        //                break;
        //            case "Size":
        //                results.Add(artifact.Size as T);
        //                break;
        //            case "SuspectedFileType":
        //                results.Add(artifact.SuspectedFileType as T);
        //                break;
        //        }
        //    }
        //    return results.Where(r => (r != null));
        //}
    }
}