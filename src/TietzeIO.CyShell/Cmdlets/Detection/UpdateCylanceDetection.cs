using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyAPI.Rest;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Detection
{
    /// <summary>
    /// Updates a detection. Modeled after GetCylanceDetectionAsync. Contains "cache mode" extended functionality.
    /// </summary>
    [Cmdlet(VerbsData.Update, "CylanceDetection")]
    [OutputType(typeof(CyRestDetectionUpdateResponse))]
    public class UpdateCylanceDetection : CyParallelPipelineCmdlet<List<CyRestDetectionUpdateResponse>, ICyDetection>
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyDetectionMetaData Detection { get; set; }
        [Parameter]
        public string Comment { get; set; }
        [Parameter]
        [ValidateSet("New", "False Positive", "Follow Up", "In Progress", "Reviewed", "Done")]
        public string Status { get; set; }

        public UpdateCylanceDetection()
        {
            EnableProgressInfo(PowershellModuleConstants.ACTIVITY_KEY_UPDATE_DETECTION,
                "Updating detection(s)");
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ObjectsPerTransaction = 50; //PowershellModuleConstants.MAX_ITEMS_PER_UPDATEDETECTION_TRANSACTION;
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            QueueObject(Detection);
        }

        protected override Task[] CreateTasksFromObjects(ICyDetection[] q)
        {
            return new[] { Connection.UpdateDetectionAsync(q, Status, Comment) };
        }

        protected override List<CyRestDetectionUpdateResponse> ResultObjectTransformation(List<CyRestDetectionUpdateResponse> o)
        {
            // suppress success
            return o?.Where(x => !x.IsSuccess).Select(x => x).ToList();
        }
    }
}
