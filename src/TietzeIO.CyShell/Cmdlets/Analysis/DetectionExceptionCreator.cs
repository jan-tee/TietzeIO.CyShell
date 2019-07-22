using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.CAE;
using TietzeIO.CyAPI.CAE.RuleElements;
using TietzeIO.CyAPI.CAE.RuleElements.Operands.Base;
using TietzeIO.CyAPI.CAE.RuleElements.Operands.DataOperand;
using TietzeIO.CyAPI.CAE.RuleElements.Operands.LiteralSet;
using TietzeIO.CyAPI.CAE.RuleElements.Operators;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyAPI.Configuration.Data;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    public class DetectionExceptionCreator
    {
        // detection exception magic values
        private const string MAGIC_TAG_AUTOCREATED_EXCEPTION = "CyAPIv2PS";
        // detection magic values
        private const string MAGIC_COMMENT_REVIEWED = "This detection was marked as 'Reviewed' because associated artifacts were marked as excluded in the exception rule '{0}'";

        public enum ExclusionType
        {
            InstigatingProcessName,
            InstigatingProcessHash,
            TargetProcessName,
            TargetProcessHash,
            TargetFileName
        }

        private List<DetectionAnalysisRecord> Output;

        /// <summary>
        /// A result record to be returned
        /// </summary>
        public class DetectionAnalysisRecord
        {
            public string Item { get; set; }
            public long Count { get => RawDetections.Count(); }
            public Dictionary<string, long> AssociatedImageFileHashes;
            public Dictionary<string, long> AssociatedOtherHashes;
            public List<CyDetection> RawDetections;
            public List<Process> RawMatches;

            public DetectionAnalysisRecord()
            {
                RawMatches = new List<Process>();
                RawDetections = new List<CyDetection>();
                AssociatedImageFileHashes = new Dictionary<string, long>();
                AssociatedOtherHashes = new Dictionary<string, long>();
            }

            public void AssociateProcessHash(string hash)
            {
                if (!AssociatedImageFileHashes.ContainsKey(hash))
                {
                    AssociatedImageFileHashes.Add(hash, 1);
                }
                else
                {
                    AssociatedImageFileHashes[hash]++;
                }
            }

            public void AssociateOtherHash(string hash)
            {
                if (!AssociatedOtherHashes.ContainsKey(hash))
                {
                    AssociatedOtherHashes.Add(hash, 1);
                }
                else
                {
                    AssociatedOtherHashes[hash]++;
                }
            }
        }

        /// <summary>
        /// Models ONE execution, and serves as input for data analysis
        /// </summary>
        public class Process
        {
            public CyDetectionRuleMetaData Rule { get; set; }
            private string _path;
            public string ProcessImagePath
            {
                get => _path; set
                {
                    _path = value;
                    ProcessPathNormalized = NormalizePath(value);
                }
            }
            private string _commandLine;
            public string ProcessCommandLine
            {
                get => _commandLine; set
                {
                    _commandLine = value;
                    ProcessCommandlineNormalized = NormalizeCommandLine(value);
                }
            }
            public string ProcessName { get; }
            public string ProcessImageSHA256 { get; }
            public string ProcessPathNormalized { get; set; }
            public string ProcessCommandlineNormalized { get; private set; }

            private string _targetFilePath;
            public string TargetFileSHA256;
            public string TargetFilePath
            {
                get => _targetFilePath;
                set
                {
                    _targetFilePath = value;
                    TargetFilePathNormalized = NormalizePath(value);
                }
            }
            public string TargetFilePathNormalized { get; private set; }

            public string State { get; set; }
            public CyDetection Detection { get; set; }

            public Process(string cmdline, string path, string name, string sha256, Guid detectionRuleId, int detectionRuleVersion)
            {
                ProcessImagePath = path;
                ProcessCommandLine = cmdline;
                ProcessName = name;
                ProcessImageSHA256 = sha256;
            }
        }

        public static string NormalizeCommandLine(string cmdline)
        {
            int numMatches = 0;
            foreach (var pnr in CyAPI.Configuration.ConfigurationData.Default.DetectionsAnalyzerRules.NormalizationRules.Values.Where(r => r.RuleType.Equals(NormalizationRule.RULETYPE_CMDLINE)))
            {
                if (pnr.TryNormalize(cmdline, out var o))
                {
                    // apply as many normalizations as possible
                    cmdline = o;
                    numMatches++;
                }
            }
            return cmdline;
        }

        public static string NormalizePath(string path)
        {
            int numMatches = 0;
            foreach (var pnr in CyAPI.Configuration.ConfigurationData.Default.DetectionsAnalyzerRules.NormalizationRules.Values.Where(r => r.RuleType.Equals(NormalizationRule.RULETYPE_PATH)))
            {
                if (pnr.TryNormalize(path, out var o))
                {
                    // apply as many normalizations as possible
                    path = o;
                    numMatches++;
                }
            }
            return path;
        }

        public void InteractiveCreateDetectionExceptions(CyApiCmdlet cmdlet, string[] ruleNames)
        {
            cmdlet.WriteLine("Let's walk through the grouped artifacts one by one, and decide whether to exclude them from future detection events. If you have run this" +
                " wizard before, it will detect and amend the existing detection exception. If you have not run this wizard before, it will create a detection exception" +
                " for each rule, and add individual artifacts that you select to the exclusion. It will also prompt you to update existing detections matching the artifact," +
                " so that they can be marked as 'Reviewed' and ignored, allowing you to focus on 'New' detections.");
            cmdlet.WriteLineHL("Let's start mass processing detections.");

            foreach (var rule in ruleNames)
            {
                cmdlet.WriteVerbose("Creating exceptions: Identifying exception object");
                var name = $"{rule}: List of allowed processes by {"hash"}";

                var isDirty = false;
                var exception = (from e in cmdlet.Api.DetectionExceptionsDetail
                                 where name.Equals(e.Name, StringComparison.InvariantCultureIgnoreCase) && (e.Tags != null) && e.Tags.Contains(MAGIC_TAG_AUTOCREATED_EXCEPTION, StringComparer.InvariantCultureIgnoreCase)
                                 select e).SingleOrDefault();
                if (exception == null)
                {
                    cmdlet.WriteVerbose($"Creating new in-memory detection exception rule: {name}");
                    exception = CyDetectionException.GetNewExceptionTemplate(name);

                    var state = new State("Exception");
                    exception.States.Add(state);
                    var literalSetString = new LiteralSetString();
                    var fo = new EqualsAnyOperator(
                        new StringDataOperand("InstigatingProcessImageFile", "Sha256Hash"),
                        new IOperand[]
                        {
                            literalSetString
                        },
                        TietzeIO.CyAPI.CAE.Configurability.RuleOperandType.String
                    );
                    state.AddFieldOperator("0", fo);
                    state.Function = "0";

                    exception.Tags.Add(MAGIC_TAG_AUTOCREATED_EXCEPTION);
                }
                else
                {
                    cmdlet.WriteVerbose($"Found existing exception rule: {name} ({exception.id.Value})");

                    var fo = exception.States[0].FieldOperators["0"].Operands[1] as LiteralSetString;
                    cmdlet.WriteVerbose($"Found operand: {fo}");
                }

                var hashSet = (from o in exception.States[0].FieldOperators["0"].Operands where o is LiteralSetString select o).Single() as LiteralSetString;
                var reviewedDetectionsQueue = new List<CyDetection>();
                var reviewedDetectionsComment = string.Format(MAGIC_COMMENT_REVIEWED, exception.Name);

                // create content
                var stopProcessing = false;

                foreach (var output in Output)
                {
                    if (stopProcessing)
                    {
                        break;
                    }
                    cmdlet.WriteLine();
                    cmdlet.WriteLine($"ARTIFACT: {output.Item}, {output.Count} Hits");

                    foreach (var hashKvp in output.AssociatedImageFileHashes)
                    {
                        var hash = hashKvp.Key;
                        if (!stopProcessing)
                        {
                            if (!hashSet.Data.Contains(hash, StringComparer.InvariantCultureIgnoreCase))
                            {
                                cmdlet.WriteLine("Add hash  to the set of excluded hashes (y/n/x for end)?");
                                var answer = cmdlet.Host.UI.ReadLine();
                                switch (answer.ToLowerInvariant())
                                {
                                    case "y":
                                        cmdlet.Write("Add hash ");
                                        hashSet.Data.Add(hash);
                                        isDirty = true;
                                        cmdlet.WriteLineHL($"{hash}");

                                        cmdlet.WriteLine($"Do you also want to add all detections for rule '{rule}' with this artifact as 'Reviewed' and add a standard comment?");
                                        var answer2 = cmdlet.Host.UI.ReadLine();
                                        if (answer2.ToLowerInvariant().Equals("y"))
                                        {
                                            reviewedDetectionsQueue.AddRange(output.RawDetections);
                                            cmdlet.WriteLine($"Queued {output.RawDetections.Count:d0} detections to be marked as 'Reviewed' and adding comment '{reviewedDetectionsComment}'");
                                        }
                                        break;
                                    case "x":
                                        stopProcessing = true;
                                        break;
                                    case "n":
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                cmdlet.WriteLine($"Hash {hash} is already contained in exception set.");
                            }
                        }
                    }
                }

                if (isDirty)
                {
                    if (!exception.id.HasValue)
                    {
                        // new exception rule - commit + refresh cache
                        if (cmdlet.ShouldProcess("Create detection exception rule?"))
                        {
                            cmdlet.WriteLine($"Creating new exception rule {exception.Name} in console.");
                            exception = cmdlet.Connection.NewDetectionExceptionAsync(exception).WaitAndUnwrapException();
                            cmdlet.WriteLineHL($" Done.");
                        }
                    }
                    else
                    {
                        if (cmdlet.ShouldProcess("Update detection exception rule?"))
                        {
                            cmdlet.WriteLine($"Updating exception rule {exception.Name} ({exception.id.Value}) in console... ");
                            exception = cmdlet.Connection.UpdateDetectionExceptionAsync(exception).WaitAndUnwrapException();
                            cmdlet.WriteLineHL(" Done.");
                        }
                    }
                    if (exception.id.HasValue)
                    {
                        // success - mark as reviewed
                        List<Task<List<CyRestDetectionUpdateResponse>>> tasks = new List<Task<List<CyRestDetectionUpdateResponse>>>();

                        int detectionsCount = 0;
                        while (reviewedDetectionsQueue.Count > 0)
                        {
                            // get up to max # per tx items
                            var count = Math.Min(PowershellModuleConstants.MAX_ITEMS_PER_UPDATEDETECTION_TRANSACTION, reviewedDetectionsQueue.Count);
                            detectionsCount += count;
                            var items = reviewedDetectionsQueue.GetRange(0, count);
                            reviewedDetectionsQueue.RemoveRange(0, count);
                            if (cmdlet.ShouldProcess(
                                $"Update {items.Count} detections ({string.Join(", ", items.Select(s => s.PhoneticId))})?"))
                            {
                                tasks.Add(cmdlet.Connection.UpdateDetectionAsync(items.ToArray(), "Reviewed", reviewedDetectionsComment));
                            }
                        }

                        PowershellConsoleUtil.BlockWithProgress(cmdlet,
                            tasks.ConvertAll<Task>(x => x as Task),
                            PowershellModuleConstants.ACTIVITY_KEY_BULK_UPDATING_DETECTIONS,
                            "Updating detections",
                            $"Updating {detectionsCount} detections in {tasks.Count()} individual transactions, and setting to 'Reviewed'");

                        foreach (var output in tasks)
                        {
                            cmdlet.WriteObject(output.Result, true);
                        }

                        // only refresh when cmdlet.Write was successful
                        cmdlet.WriteVerbose("Refreshing detection exceptions cache");
                        cmdlet.Api.RefreshDetectionExceptionsCache(cmdlet.Connection);
                    }
                }
            }
        }
    }
}