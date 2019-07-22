using System;
using System.Collections.Generic;
using System.Text;

namespace TietzeIO.CyShell.Util
{
    class PowershellModuleConstants
    {
        public const int ACTIVITY_KEY_REFRESH_DETECTIONS = 100;
        public const int ACTIVITY_KEY_BULK_UPDATING_DETECTIONS = 101;
        public const int ACTIVITY_KEY_ANALYSIS = 102;
        public const int ACTIVITY_KEY_ADD_REMOVE_DEVICES_IN_ZONES = 103;
        public const int ACTIVITY_KEY_GLOBAL_LIST_OP = 104;
        public const int ACTIVITY_KEY_REMOVE_DETECTION = 105;
        public const int ACTIVITY_KEY_UPDATE_DETECTION = 106;
        public const int ACTIVITY_KEY_REMOVE_DEVICE = 107;
        public const int ACTIVITY_KEY_DOWNLOAD_TDR = 108;
        public const int ACTIVITY_KEY_CONVERT_TDR = 109;
        public const int ACTIVITY_KEY_GET_THREATDEVICES = 110;
        public const int ACTIVITY_KEY_GET_DEVICE = 111;
        public const int ACTIVITY_KEY_REFRESH_CACHE_DETECTIONS = 120;
        public const int ACTIVITY_KEY_GET_LOCKDOWN_STATUS = 130;
        public const int ACTIVITY_KEY_GET_DETECTION = 140;
        public const int ACTIVITY_KEY_BLOCK_THREAT = 150;
        public const int ACTIVITY_KEY_UNBLOCK_THREAT = 151;
        public const int ACTIVITY_KEY_GET_THREAT = 152;

        public const double UPDATE_PROGRESS_INTERVAL = 0.5;

        // number of "update detection" items to group together in one "update detections" transaction.
        // actual API limit is unknown, 100 seems safe and a reasonable trade-off...
        public const int MAX_ITEMS_PER_UPDATEDETECTION_TRANSACTION = 100;
    }
}
