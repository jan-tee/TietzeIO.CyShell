using System;
using System.Collections.Generic;
using System.Linq;
using TietzeIO.CyAPI.Entities.Optics;

namespace TietzeIO.CyShell.Util
{
    public class ArtifactsOfInterestFilter
    {
        public string[] StateFilter { get; set; }
        public string[] SourceFilter { get; set; }
        public string[] TypeFilter { get; set; }

        //public DetectionFilter DetectionFilter { get; set; }

        public List<CyDetection.CyArtifact> Filter(CyDetection Detection)
        {
            var results = new List<CyDetection.CyArtifact>();
            if (Detection.ArtifactsOfInterest == null)
            {
                return results;
            }
            foreach (var stateArtifacts in Detection.ArtifactsOfInterest)
            {
                string state = stateArtifacts.Key;
                List<CyDetection.CyArtifactOfInterest> aois = stateArtifacts.Value;
                foreach (var aoi in aois)
                {
                    var source = aoi.Source;
                    var type = aoi.Artifact.Type;
                    var artifact = Detection.ResolveArtifactReference(aoi.Artifact.Uid);

                    bool includeInResults = true;
                    if (null != StateFilter)
                    {
                        // filter by state
                        includeInResults &= StateFilter.Contains(state, StringComparer.InvariantCultureIgnoreCase);
                    }
                    if (null != SourceFilter)
                    {
                        // filter by source
                        includeInResults &= SourceFilter.Contains(source, StringComparer.InvariantCultureIgnoreCase);
                    }
                    if (null != TypeFilter)
                    {
                        // filter by AOI type
                        includeInResults &= TypeFilter.Contains(type, StringComparer.InvariantCultureIgnoreCase);
                    }

                    if (includeInResults)
                    {
                        results.Add(artifact);
                    }

                    //if (includeInResults)
                    //{
                    //    ////if (null != Facet)
                    //    ////{
                    //    ////    new PSCustomObject().
                    //    ////    // output facet only
                    //    ////    switch (Facet)
                    //    ////    {
                    //    ////        case "Name":
                    //    ////            WriteObject(artifact.Name);
                    //    ////            break;
                    //    ////        case "Path":
                    //    ////            WriteObject(artifact.Path);
                    //    ////            break;
                    //    ////        case "CommandLine":
                    //    ////            WriteObject(artifact.CommandLine);
                    //    ////            break;
                    //    ////        case "Domain":
                    //    ////            WriteObject(artifact.Domain);
                    //    ////            break;
                    //    ////        case "Sha256Hash":
                    //    ////            WriteObject(artifact.Sha256Hash);
                    //    ////            break;
                    //    ////        case "Md5Hash":
                    //    ////            WriteObject(artifact.Md5Hash);
                    //    ////            break;
                    //    ////        case "Size":
                    //    ////            WriteObject(artifact.Size);
                    //    ////            break;
                    //    ////        case "SuspectedFileType":
                    //    ////            WriteObject(artifact.SuspectedFileType);
                    //    ////            break;
                    //    ////    }
                    //    ////}
                    //    ////else
                    //    //{
                    //    // output whole artifact object
                    //}
                }
            }
            return results;
        }
    }
}