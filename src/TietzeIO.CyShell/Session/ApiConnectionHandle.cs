using Nito.AsyncEx.Synchronous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyAPI.Session;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Session
{
    /// <summary>
    /// Shared session state.
    /// 
    /// This does not contain the actual APIv2 object, but only a serialized version describing how to set up an actual API session,
    /// and some cached artifacts like detections etc.
    /// 
    /// The session itself is not thread-safe, and must/will be instantiated for each Powershell Cmdlet invocation.
    /// </summary>
    public class ApiConnectionHandle
    {
        private static ApiConnectionHandle _global;
        public static ApiConnectionHandle Global { get => _global; }

        public SessionDescriptor Session { get; private set; }

        internal ApiV2 API { get; set; }
        public bool CacheMode { get; internal set; }
        public bool IncludeOpticsData { get; set; } = true;
        public bool IncludeProtectData { get; set; } = true;
        public List<CyDeviceMetaData> Devices { get; private set; }
        public List<CyThreatMetaData> Threats { get; private set; }
        public List<CyThreatDeviceEnriched> ThreatDevices { get; private set; }
        public List<CyDetectionMetaData> Detections { get; private set; }
        public List<CyDetection> DetectionsDetail { get; private set; }
        public List<CyDetectionRuleMetaData> DetectionRules { get; private set; }
        public List<CyDetectionException> DetectionExceptionsDetail { get; private set; }
        public List<CyPolicyMetaData> Policies { get; private set; }
        public List<CyUser> Users { get; private set; }

        public ApiConnectionHandle(SessionDescriptor session)
        {
            Session = session;
        }

        public void MakeGlobal()
        {
            _global = this;
        }

        public static void ClearGlobal()
        {
            _global = null;
        }

        public void RefreshPolicyCache(ApiV2 connection)
        {
            Policies = connection.
                GetPoliciesAsync().
                WaitAndUnwrapException().
                OrderBy(p => p.name).
                ToList();
        }

        public void RefreshDetectionCache(ApiV2 connection)
        {
            Detections = connection.
                GetDetectionsAsync().
                WaitAndUnwrapException().
                OrderBy(d => d.OccurrenceTime).
                ToList();
        }

        public void RefreshDetectionDetailsCache(ApiV2 connection, CyCmdlet cmdlet)
        {
            DetectionsDetail = new List<CyDetection>();

            List<Task<CyDetection>> tasks = new List<Task<CyDetection>>(Detections.Count);
            var newDetections = (from d in Detections where d.Status.Equals("NEW", StringComparison.InvariantCultureIgnoreCase) select d);
            foreach (var detection in newDetections)
            {
                var task = connection.GetDetectionAsync(detection.id);
                tasks.Add(task);
            }

            if (newDetections.Any())
            {
                PowershellConsoleUtil.BlockWithProgress(cmdlet,
                    tasks.ConvertAll<Task>(x => x as Task),
                    PowershellModuleConstants.ACTIVITY_KEY_REFRESH_DETECTIONS,
                    "Downloading detections detail",
                    $"Downloading full record for all {newDetections.Count()} NEW detections");
            }

            foreach (var task in tasks)
            {
                if (task.IsCompleted)
                    DetectionsDetail.Add(task.Result);
            }
        }

        public void RefreshDetectionRulesCache(ApiV2 connection)
        {
            DetectionRules = new List<CyDetectionRuleMetaData>(connection.
                GetDetectionRulesAsync().
                WaitAndUnwrapException().
                OrderBy(d => d.Name));
        }

        public void RefreshDetectionExceptionsCache(ApiV2 connection)
        {
            DetectionExceptionsDetail = new List<CyDetectionException>();
            var list = new List<CyDetectionExceptionMetaData>(connection.
                GetDetectionExceptionsAsync().
                WaitAndUnwrapException().
                OrderBy(d => d.Name));
            list.ForEach(d =>
            {
                DetectionExceptionsDetail.Add(connection.
                    GetDetectionExceptionAsync(d.id.Value).
                    WaitAndUnwrapException());
            });
        }

        public void RefreshThreatInstancesCache(ApiV2 connection, CyCmdlet cmdlet)
        {
            ThreatDevices = new List<CyThreatDeviceEnriched>();

            List<Task<List<CyThreatDeviceEnriched>>> tasks = new List<Task<List<CyThreatDeviceEnriched>>>(ThreatDevices.Count * 3);
            var unhandledThreats = from t in Threats where (!t.global_quarantined) && (!t.safelisted) select t;
            foreach (var threat in unhandledThreats)
            {
                var task = connection.RequestThreatDevicesAsync(threat);
                tasks.Add(task);
            }

            if (unhandledThreats.Count() > 0)
            {
                PowershellConsoleUtil.BlockWithProgress(cmdlet,
                    tasks.ConvertAll<Task>(x => x as Task),
                    PowershellModuleConstants.ACTIVITY_KEY_REFRESH_DETECTIONS,
                    "Downloading threat instances detail",
                    $"Downloading path and device information for all {unhandledThreats.Count()} unhandled threats");
            }

            foreach (var task in tasks)
            {
                ThreatDevices.AddRange(task.Result);
            }
        }

        public void RefreshThreatCache(ApiV2 connection)
        {
            Threats = connection.
                RequestThreatListAsync().
                WaitAndUnwrapException().
                OrderBy(d => d.name).
                ToList();
        }

        public void RefreshDeviceCache(ApiV2 connection)
        {
            Devices = connection.
                RequestDeviceListAsync().
                WaitAndUnwrapException().
                OrderBy(d => d.device_name).
                ToList();
        }

        public void RefreshUserCache(ApiV2 connection)
        {
            Users = connection.
                RequestUserListAsync().
                WaitAndUnwrapException().
                OrderBy(d => d.email).
                ToList();
        }

        public void RefreshCache(ApiV2 connection, CyCmdlet cmdlet)
        {
            cmdlet.Write("Refreshing device cache... ");
            RefreshDeviceCache(connection);
            cmdlet.WriteLineHL($"Done ({Devices.Count}).");

            ClearProtectCache();
            if (IncludeProtectData)
            {
                cmdlet.Write("Refreshing threats cache... ");
                RefreshThreatCache(connection);
                cmdlet.WriteLineHL($"Done ({Threats.Count}).");

                cmdlet.Write("Refreshing threat instances cache... ");
                RefreshThreatInstancesCache(connection, cmdlet);
                cmdlet.WriteLineHL($"Done ({Threats.Count}).");
            }

            ClearOpticsCache();
            if (IncludeOpticsData)
            {
                cmdlet.Write("Refreshing detection rules...");
                RefreshDetectionRulesCache(connection);
                cmdlet.WriteLineHL($"Done ({DetectionRules.Count}).");

                cmdlet.Write("Refreshing detection exceptions rules... ");
                RefreshDetectionExceptionsCache(connection);
                cmdlet.WriteLineHL($"Done ({DetectionExceptionsDetail.Count}).");

                cmdlet.Write("Refreshing detections cache... ");
                RefreshDetectionCache(connection);
                cmdlet.WriteLineHL($"Done ({Detections.Count}).");

                cmdlet.Write("Refreshing detections details cache... ");
                RefreshDetectionDetailsCache(connection, cmdlet);
                cmdlet.WriteLineHL($"Done ({DetectionsDetail.Count}).");
            }

            cmdlet.Write("Refreshing policy cache... ");
            RefreshPolicyCache(connection);
            cmdlet.WriteLineHL($"Done ({Policies.Count}).");
            cmdlet.Write("Refreshing user cache... ");
            RefreshUserCache(connection);
            cmdlet.WriteLineHL($"Done ({Users.Count}).");
        }

        public CyDetectionRuleMetaData GetDetectionRule(Guid id)
        {
            var result = from r in DetectionRules
                         where r.id.Equals(id)
                         select r;
            if (result.Count() > 0)
                return result.First();
            else return null;
        }

        public void ClearCache(CyCmdlet cmdlet)
        {
            cmdlet.Write("Clearing cache... ");
            Devices = new List<CyDeviceMetaData>();
            ClearProtectCache();
            ClearOpticsCache();
            Policies = new List<CyPolicyMetaData>();
            Users = new List<CyUser>();
            cmdlet.WriteLineHL("Done.");
        }

        private void ClearProtectCache()
        {
            Threats = new List<CyThreatMetaData>();
            ThreatDevices = new List<CyThreatDeviceEnriched>();
        }

        private void ClearOpticsCache()
        {
            Detections = new List<CyDetectionMetaData>();
            DetectionsDetail = new List<CyDetection>();
            DetectionExceptionsDetail = new List<CyDetectionException>();
            DetectionRules = new List<CyDetectionRuleMetaData>();
        }
    }
}
