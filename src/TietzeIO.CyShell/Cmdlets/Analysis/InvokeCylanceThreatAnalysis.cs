using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Analysis
{
    [Cmdlet(VerbsLifecycle.Invoke, "CylanceThreatAnalysis")]
    [OutputType(typeof(CyDeviceMetaData))]
    public partial class InvokeCylanceThreatAnalysis : CyCachedModeCmdlet
    {
        [Parameter(Mandatory = false)]
        [ValidateSet("Classification", "Count")]
        public string OrderBy { get; set; }

        public InvokeCylanceThreatAnalysis()
        {
            RequireOpticsCache = false;
            RequireProtectCache = true;
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            var c = (from t in Api.Threats
                     group t by (t.GetClassificationString()) into g
                     select new CylanceThreatAnalysisRecord(g.Key, g.AsEnumerable()));

            if (c.Any())
            {
                switch (OrderBy)
                {
                    case "Classification":
                        c = from tr in c orderby tr.Classification ascending select tr;
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
