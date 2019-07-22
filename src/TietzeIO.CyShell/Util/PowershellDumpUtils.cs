using TietzeIO.CyAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Util
{
    public class PowershellDumpUtil
    {
        public static void DumpDetection(CyCmdlet cmdlet, CyDetection d)
        {
            cmdlet.WriteLine("-------------------------------------------------------------------------------------------------------------");
            cmdlet.Write("Detection ");
            cmdlet.WriteLineHL($"'{d.DetectionRule.Name}' (v{d.DetectionRule.Version}) [{d.PhoneticId}]");
            if (d.ArtifactsOfInterest != null)
            {
                cmdlet.Write("State progression from artifacts: ");
                cmdlet.WriteLineHL($"{string.Join(" > ", d.ArtifactsOfInterest.Keys)}");

                cmdlet.WriteLine("Raw artifacts: ");
                foreach (var a in d.AssociatedArtifacts)
                {
                    cmdlet.WriteLine($"{a}");
                }
                if (d.ArtifactsOfInterest != null)
                    foreach (var state in d.ArtifactsOfInterest.Keys)
                    {
                        cmdlet.Write("Collected artifacts for state: ");
                        cmdlet.WriteLineHL($"{state}");

                        foreach (var collected_artifact in d.ArtifactsOfInterest[state])
                            cmdlet.WriteLine(
                                $"{d.ResolveArtifactReference(collected_artifact.Artifact)} of type {d.ResolveArtifactReference(collected_artifact.Artifact).ArtifactType}, source {collected_artifact.Source}");
                    }
            }
        }
    }
}
