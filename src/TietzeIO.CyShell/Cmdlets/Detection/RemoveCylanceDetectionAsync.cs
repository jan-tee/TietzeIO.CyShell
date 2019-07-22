using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using System.Collections.Generic;
using System.Linq;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Detection
{
    /// <summary>
    /// Removes a detection. Modeled after GetCylanceDetectionAsync. Contains "cache mode" extended functionality.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "CylanceDetection")]
    [OutputType(typeof(CyRestDetectionUpdateResponse))]
    public class RemoveCylanceDetection : CyParallelPipelineCmdlet<List<CyRestDetectionUpdateResponse>, CyDetectionMetaData>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyDetectionMetaData Detection { get; set; }

        public RemoveCylanceDetection()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_REMOVE_DETECTION,
                "Removing detection(s)");
        }

        protected override Task[] CreateTasksFromObjects(CyDetectionMetaData[] q)
        {
            return new[] { Connection.RemoveDetectionAsync(q[0].id) };
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            QueueObject(Detection);
        }

        protected override List<CyRestDetectionUpdateResponse> ResultObjectTransformation(List<CyRestDetectionUpdateResponse> o)
        {
            // suppress success
            return o?.Where(x => !x.IsSuccess).Select(x => x).ToList();
        }
    }
}