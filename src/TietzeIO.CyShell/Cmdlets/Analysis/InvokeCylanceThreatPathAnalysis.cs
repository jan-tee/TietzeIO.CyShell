using System.Linq;
using System.Management.Automation;
using System.Text.RegularExpressions;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    [Cmdlet(VerbsLifecycle.Invoke, "CylanceThreatPathAnalysis")]
    [OutputType(typeof(CyDeviceMetaData))]
    public partial class InvokeCylanceThreatPathAnalysis : CyCachedModeCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = false)]
        [ValidateSet("Path", "Count")]
        public string OrderBy { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet("Unsafe", "Abnormal", "Waived", "Default")]
        public string FileStatus { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter IgnoreRecycleBin { get; set; }

        //[Parameter(Mandatory = false)]
        //public string[] IncludePath { get; set; }

        private PowershellDynamicParameter DeviceNameParam;
        private PowershellDynamicParameter ClassificationParam;

        public object GetDynamicParameters()
        {
            if (!Api.CacheMode) return null;

            DeviceNameParam = new PowershellDynamicParameter(this, "Device", (from d in Api.Devices select d.device_name).ToArray());
            DeviceNameParam.AllowMultiple = true;
            DeviceNameParam.Position = 1;

            var classes = (from c in Api.Threats select c.GetClassificationString());
            string[] classifications = new string[] { };
            if (classes.Any())
            {
                classifications = classes.Distinct().OrderBy(d => d).ToArray();
            }
            ClassificationParam = new PowershellDynamicParameter(this, "Classification", classifications);
            ClassificationParam.AllowMultiple = true;
            ClassificationParam.Position = 1;

            var dynamicParams = DeviceNameParam.GetParamDictionary();
            ClassificationParam.AddToParamDictionary(dynamicParams);
            return dynamicParams;
        }

        public InvokeCylanceThreatPathAnalysis()
        {
            RequireOpticsCache = false;
            RequireProtectCache = true;
        }

        private Regex _excludeRecycleBinRegex = new Regex(@"[c-z]:\\\$Recycle.Bin\\", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private bool IsPathIncluded(string path)
        {
            if (!IgnoreRecycleBin)
            {
                return true;
            }
            else
            {
                return !_excludeRecycleBinRegex.IsMatch(path);
            }
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            var c = (from t in Api.ThreatDevices
                     where (IsPathIncluded(t.GetPath()))
                     group t by (t.GetPath()) into g
                     select new CylanceThreatDeviceAnalysisRecord(g.Key, g.AsEnumerable()));

            if (c.Any())
            {
                switch (OrderBy)
                {
                    case "Path":
                        c = from tr in c orderby tr.Path ascending select tr;
                        break;
                    default:
                        c = from tr in c orderby tr.Count descending select tr;
                        break;
                }
                foreach (var r in c)
                {
                    WriteObject(r);
                }
            }
        }
    }
}
