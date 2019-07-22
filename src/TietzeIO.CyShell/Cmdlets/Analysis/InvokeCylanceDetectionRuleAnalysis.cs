using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
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
    /// <summary>
    /// Performs an analysis of cached (full) detection records.
    /// 
    /// Will cluster based on:
    /// - normalized versions of command lines
    /// </summary>
    [Cmdlet(VerbsLifecycle.Invoke, "CylanceDetectionRuleAnalysis", SupportsShouldProcess = true)]
    [OutputType(typeof(object))]
    public class InvokeCylanceDetectionRuleAnalysis : CyCachedModeCmdlet, IDynamicParameters
    {
        // argument values for "Mode" argument
        private const string MODE_RULE_FREQUENCY = "Rule/Frequency";
        private const string MODE_PATH_INSTIGATING = "Path/Instigating Process";
        private const string MODE_HASH_INSTIGATING = "Hash/Instigating Process";
        private const string MODE_HASH_TARGET = "Hash/Target Process";
        private const string MODE_PATH_TARGET = "Path/Target Process";
        private const string MODE_FILE_TARGET = "File/Target";

        // detection exception magic values
        private const string MAGIC_TAG_AUTOCREATED_EXCEPTION = "CyAPIv2PS";
        // detection magic values
        private const string MAGIC_COMMENT_REVIEWED = "This detection was marked as 'Reviewed' because associated artifacts were marked as excluded in the exception rule '{0}'";

        [Parameter(Mandatory = false)]
        [ValidateSet(MODE_RULE_FREQUENCY, MODE_PATH_INSTIGATING, MODE_HASH_INSTIGATING, MODE_HASH_TARGET, MODE_PATH_TARGET, MODE_FILE_TARGET)]
        [PSDefaultValue(Value = MODE_RULE_FREQUENCY)]
        public string Mode { get; set; }

        private const string SRC_TARGET_PROCESS = "Target Process";
        private const string SRC_TARGET_PROCESS_IMAGE_FILE = "Target Process Image File";
        private const string SRC_INSTIGATING_PROCESS = "Instigating Process";
        private const string SRC_INSTIGATING_PROCESS_IMAGE_FILE = "Instigating Process Image File";
        private const string SRC_TARGET_FILE = "Target File";

        [Parameter(Mandatory = false)]
        public SwitchParameter CreateExceptions { get; set; }

        private PowershellDynamicParameter RuleNameParam;
        private PowershellDynamicParameter DeviceNameParam;

        private List<DetectionAnalysisRecord> Output;

        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;

            DeviceNameParam = new PowershellDynamicParameter(this, "Device", (from d in Api.Devices select d.device_name).ToArray());
            DeviceNameParam.AllowMultiple = true;

            RuleNameParam = new PowershellDynamicParameter(this, "Rule", (from d in Api.DetectionRules select d.Name).ToArray());
            RuleNameParam.AllowMultiple = true;

            var dynamicParams = DeviceNameParam.GetParamDictionary();
            RuleNameParam.AddToParamDictionary(dynamicParams);
            return dynamicParams;
        }

        public InvokeCylanceDetectionRuleAnalysis()
        {
            RequireOpticsCache = true;
            RequireProtectCache = true;
        }

        private bool IsMatch(CyDetection detection)
        {
            bool isMatch = true;

            if (RuleNameParam.Values != null)
            {
                isMatch &= (detection.DetectionRule != null) && RuleNameParam.Values.Contains(detection.DetectionRuleName, StringComparer.InvariantCultureIgnoreCase);
            }

            if (DeviceNameParam.Values != null)
            {
                isMatch &= (detection.Device != null) && DeviceNameParam.Values.Contains(detection.Device.name, StringComparer.InvariantCultureIgnoreCase);
            }

            return isMatch;
        }

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

        protected void ValidateParameters()
        {
            if ((!string.IsNullOrEmpty(Mode)) && (RuleNameParam.Values == null))
            {
                // -Match Fuzzy|Hash|etc. but no -Rule parameter given
                //throw new Exception("When an analysis mode is specified, you also need to specify a rule to analyze.");
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ValidateParameters();

            Output = new List<DetectionAnalysisRecord>();

            var detections = from d in Api.DetectionsDetail
                             where ((d != null) && IsMatch(d))
                             select d;

            string process;
            string file;
            switch (Mode)
            {
                case MODE_PATH_INSTIGATING:
                    process = SRC_INSTIGATING_PROCESS;
                    file = SRC_INSTIGATING_PROCESS_IMAGE_FILE;
                    AnalyzeRuleByPathFuzzy(detections, process, file);
                    break;
                case MODE_HASH_INSTIGATING:
                    process = SRC_INSTIGATING_PROCESS;
                    file = SRC_INSTIGATING_PROCESS_IMAGE_FILE;
                    AnalyzeRuleByHash(detections, process, file);
                    break;
                case MODE_FILE_TARGET:
                    AnalyzeRuleByTargetFile(detections);
                    break;
                case MODE_HASH_TARGET:
                    process = SRC_TARGET_PROCESS;
                    file = SRC_TARGET_PROCESS_IMAGE_FILE;
                    AnalyzeRuleByHash(detections, process, file);
                    break;
                case MODE_PATH_TARGET:
                    process = SRC_TARGET_PROCESS;
                    file = SRC_TARGET_PROCESS_IMAGE_FILE;
                    AnalyzeRuleByPathFuzzy(detections, process, file);
                    break;
                default:
                    AnalyzeDetectionPrevalence(detections);
                    break;
            }

            if (Output.Count > 0)
            {
                if (CreateExceptions.IsPresent)
                {
                    CreateDetectionExceptions();
                }
                else
                {
                    WriteObject((from l in Output orderby l.Count descending select l), true);
                }
            }
        }

        private void AnalyzeDetectionPrevalence(IEnumerable<CyDetection> detections)
        {
            Dictionary<string, DetectionAnalysisRecord> results = new Dictionary<string, DetectionAnalysisRecord>();

            foreach (var detection in detections)
            {
                string ruleName = detection.DetectionRule.Name;

                if (!results.ContainsKey(ruleName))
                {
                    var newDar = new DetectionAnalysisRecord();
                    newDar.Item = detection.DetectionRule.Name;
                    // add version + exception version info
                    results.Add(ruleName, newDar);
                }

                var dar = results[ruleName];

                // cycle through all processes
                if (detection.ArtifactsOfInterest != null)
                {
                    foreach (var stateArtifacts in detection.ArtifactsOfInterest)
                    {
                        var stateName = stateArtifacts.Key;
                        var aois = stateArtifacts.Value;

                        var associatedProcessFiles = (from aoi in aois
                                                      where aoi.Source.EndsWith("Process Image File")
                                                      select detection.ResolveArtifactReference(aoi.Artifact.Uid));
                        if (associatedProcessFiles.Count() > 0)
                        {
                            foreach (var file in associatedProcessFiles)
                            {
                                dar.AssociateProcessHash(file.Sha256Hash);
                            }
                        }

                        var associatedFiles = (from aoi in aois
                                               where aoi.Source.Equals("File")
                                               select detection.ResolveArtifactReference(aoi.Artifact.Uid));
                        if (associatedFiles.Count() > 0)
                        {
                            foreach (var file in associatedFiles)
                            {
                                dar.AssociateOtherHash(file.Sha256Hash);
                            }
                        }
                    }
                }

                dar.RawDetections.Add(detection);
            }

            Output.AddRange(results.Values);
        }

        private void AnalyzeRuleByTargetFile(IEnumerable<CyDetection> detections)
        {
            List<Process> Processes = new List<Process>(1000);
            foreach (var detection in detections)
            {
                // cycle through all processes
                if (detection.ArtifactsOfInterest != null)
                {
                    foreach (var stateArtifacts in detection.ArtifactsOfInterest)
                    {
                        var stateName = stateArtifacts.Key;
                        var aois = stateArtifacts.Value;

                        var processes = (from aoi in aois where aoi.Source.Equals(SRC_INSTIGATING_PROCESS, StringComparison.InvariantCultureIgnoreCase) select detection.ResolveArtifactReference(aoi.Artifact));
                        var targetFiles = (from aoi in aois where aoi.Source.Equals(SRC_TARGET_FILE, StringComparison.InvariantCultureIgnoreCase) select detection.ResolveArtifactReference(aoi.Artifact));
                        if ((targetFiles.Count() > 0) && (processes.Count() > 0))
                        {
                            var process = processes.First();
                            var targetFile = targetFiles.First();
                            CyDetection.CyArtifact processImageFile = detection.ResolveArtifactReference(process.PrimaryImage);
                            var proc = new Process(process.CommandLine, processImageFile.Path, process.Name, processImageFile.Sha256Hash, detection.DetectionRule.Id, detection.DetectionRule.Version);
                            proc.Detection = detection;
                            proc.State = stateName;
                            proc.TargetFilePath = targetFile.Path;
                            proc.TargetFileSHA256 = targetFile.Sha256Hash;
                            Processes.Add(proc);
                        }
                    }
                }
            }

            var distinctPaths = (from p in Processes select p.TargetFilePathNormalized).Distinct();
            foreach (var path in distinctPaths)
            {
                var processes = from process in Processes where process.TargetFilePathNormalized.Equals(path) select process;

                var dar = new DetectionAnalysisRecord();
                dar.Item = path;
                dar.RawMatches = new List<Process>(processes);
                dar.RawDetections = (from p in processes select p.Detection).Distinct().ToList();
                foreach (var hash in (from process in processes select process.ProcessImageSHA256))
                {
                    dar.AssociateProcessHash(hash);
                }
                foreach (var hash in (from p in processes select p.TargetFileSHA256))
                {
                    dar.AssociateOtherHash(hash);
                }
                Output.Add(dar);
            }
        }


        private void AnalyzeRuleByPathFuzzy(IEnumerable<CyDetection> detections, string sourceProcess, string sourceFile)
        {
            List<Process> Processes = new List<Process>(1000);
            foreach (var detection in detections)
            {
                // cycle through all processes
                if (detection.ArtifactsOfInterest != null)
                {
                    foreach (var stateArtifacts in detection.ArtifactsOfInterest)
                    {
                        var stateName = stateArtifacts.Key;
                        var aois = stateArtifacts.Value;

                        var processes = (from aoi in aois where aoi.Source.Equals(sourceProcess, StringComparison.InvariantCultureIgnoreCase) select detection.ResolveArtifactReference(aoi.Artifact.Uid));
                        if (processes.Count() > 0)
                        {
                            var process = processes.First();
                            CyDetection.CyArtifact processImageFile = detection.ResolveArtifactReference(process.PrimaryImage);
                            var proc = new Process(process.CommandLine, processImageFile.Path, process.Name, processImageFile.Sha256Hash, detection.DetectionRule.Id, detection.DetectionRule.Version);
                            proc.Detection = detection;
                            proc.State = stateName;
                            Processes.Add(proc);
                        }
                    }
                }
            }

            var distinctPaths = (from p in Processes select p.ProcessPathNormalized).Distinct();
            foreach (var path in distinctPaths)
            {
                var processes = from process in Processes where process.ProcessPathNormalized.Equals(path) select process;

                var dar = new DetectionAnalysisRecord();
                dar.Item = path;
                dar.RawMatches = new List<Process>(processes);
                dar.RawDetections = (from p in processes select p.Detection).Distinct().ToList();
                foreach (var hash in (from process in processes select process.ProcessImageSHA256))
                {
                    dar.AssociateProcessHash(hash);
                }

                Output.Add(dar);
            }
        }

        private void AnalyzeRuleByHash(IEnumerable<CyDetection> detections, string sourceProcess, string sourceFile)
        {
            List<Process> Processes = new List<Process>(1000);
            foreach (var detection in detections.Where(d => (d != null)))
            {
                // cycle through all processes
                if (detection.ArtifactsOfInterest != null)
                {
                    foreach (var stateArtifacts in detection.ArtifactsOfInterest)
                    {
                        var stateName = stateArtifacts.Key;
                        var aois = stateArtifacts.Value;

                        var processes = (from aoi in aois where aoi.Source.Equals(sourceProcess, StringComparison.InvariantCultureIgnoreCase) select detection.ResolveArtifactReference(aoi.Artifact.Uid));
                        if (processes.Count() > 0)
                        {
                            var process = processes.First();
                            CyDetection.CyArtifact processImageFile = detection.ResolveArtifactReference(process.PrimaryImage);
                            var proc = new Process(process.CommandLine, processImageFile.Path, process.Name, processImageFile.Sha256Hash, detection.DetectionRule.Id, detection.DetectionRule.Version);
                            proc.Detection = detection;
                            proc.State = stateName;
                            Processes.Add(proc);
                        }
                    }
                }
            }

            var distinctHashes = (from p in Processes select p.ProcessImageSHA256).Distinct();
            foreach (var sha256 in distinctHashes)
            {
                var processes = from process in Processes where process.ProcessImageSHA256.Equals(sha256) select process;

                var dar = new DetectionAnalysisRecord();
                dar.Item = sha256;
                dar.RawMatches = new List<Process>(processes);
                dar.RawDetections = (from p in processes select p.Detection).Distinct().ToList();
                foreach (var hash in (from process in processes select process.ProcessImageSHA256))
                {
                    dar.AssociateProcessHash(hash);
                }
                Output.Add(dar);
            }
        }

        private void CreateDetectionExceptions()
        {
            WriteLine("Let's walk through the grouped artifacts one by one, and decide whether to exclude them from future detection events. If you have run this" +
                " wizard before, it will detect and amend the existing detection exception. If you have not run this wizard before, it will create a detection exception" +
                " for each rule, and add individual artifacts that you select to the exclusion. It will also prompt you to update existing detections matching the artifact," +
                " so that they can be marked as 'Reviewed' and ignored, allowing you to focus on 'New' detections.");
            WriteLineHL("Let's start mass processing detections.");

            foreach (var rule in RuleNameParam.Values)
            {
                WriteVerbose("Creating exceptions: Identifying exception object");
                var name = $"{rule}: List of allowed processes by {"hash"}";

                var isDirty = false;
                var exception = (from e in Api.DetectionExceptionsDetail
                                 where name.Equals(e.Name, StringComparison.InvariantCultureIgnoreCase) && (e.Tags != null) && e.Tags.Contains(MAGIC_TAG_AUTOCREATED_EXCEPTION, StringComparer.InvariantCultureIgnoreCase)
                                 select e).SingleOrDefault();
                if (exception == null)
                {
                    WriteVerbose($"Creating new in-memory detection exception rule: {name}");
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
                    WriteVerbose($"Found existing exception rule: {name} ({exception.id.Value})");

                    var fo = exception.States[0].FieldOperators["0"].Operands[1] as LiteralSetString;
                    WriteVerbose($"Found operand: {fo}");
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
                    WriteLine();
                    WriteLine($"ARTIFACT: {output.Item}, {output.Count} Hits");

                    foreach (var hashKvp in output.AssociatedImageFileHashes)
                    {
                        var hash = hashKvp.Key;
                        if (!stopProcessing)
                        {
                            if (!hashSet.Data.Contains(hash, StringComparer.InvariantCultureIgnoreCase))
                            {
                                WriteLine("Add hash  to the set of excluded hashes (y/n/x for end)?");
                                var answer = Host.UI.ReadLine();
                                switch (answer.ToLowerInvariant())
                                {
                                    case "y":
                                        Write("Add hash ");
                                        hashSet.Data.Add(hash);
                                        isDirty = true;
                                        WriteLineHL($"{hash}");

                                        WriteLine(
                                            $"Do you also want to add all detections for rule '{rule}' with this artifact as 'Reviewed' and add a standard comment?");
                                        var answer2 = Host.UI.ReadLine();
                                        if (answer2.ToLowerInvariant().Equals("y"))
                                        {
                                            reviewedDetectionsQueue.AddRange(output.RawDetections);
                                            WriteLine(
                                                $"Queued {output.RawDetections.Count:d0} detections to be marked as 'Reviewed' and adding comment '{reviewedDetectionsComment}'");
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
                                WriteLine($"Hash {hash} is already contained in exception set.");
                            }
                        }
                    }
                }

                if (isDirty)
                {
                    if (!exception.id.HasValue)
                    {
                        // new exception rule - commit + refresh cache
                        if (ShouldProcess("Create detection exception rule?"))
                        {
                            WriteLine($"Creating new exception rule {exception.Name} in console.");
                            exception = Connection.NewDetectionExceptionAsync(exception).WaitAndUnwrapException();
                            WriteLineHL(" Done.");
                        }
                    }
                    else
                    {
                        if (ShouldProcess("Update detection exception rule?"))
                        {
                            WriteLine($"Updating exception rule {exception.Name} ({exception.id.Value}) in console... ");
                            exception = Connection.UpdateDetectionExceptionAsync(exception).WaitAndUnwrapException();
                            WriteLineHL(" Done.");
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
                            if (ShouldProcess(
                                $"Update {items.Count} detections ({string.Join(", ", items.Select(s => s.PhoneticId))})?"))
                            {
                                tasks.Add(Connection.UpdateDetectionAsync(items.ToArray(), "Reviewed", reviewedDetectionsComment));
                            }
                        }

                        PowershellConsoleUtil.BlockWithProgress(this,
                            tasks.ConvertAll<Task>(x => x as Task),
                            PowershellModuleConstants.ACTIVITY_KEY_BULK_UPDATING_DETECTIONS,
                            "Updating detections",
                            $"Updating {detectionsCount} detections in {tasks.Count()} individual transactions, and setting to 'Reviewed'");

                        foreach (var output in tasks)
                        {
                            WriteObject(output.Result, true);
                        }

                        // only refresh when write was successful
                        WriteVerbose("Refreshing detection exceptions cache");
                        Api.RefreshDetectionExceptionsCache(Connection);
                    }
                }
            }
        }
    }
}