using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace TietzeIO.CyShell.Util.Progress
{
    public class PowershellProgressInfoCalculator : ProgressInfoCalculator
    {
        public const int PROGRESSBAR_UPDATE_FREQUENCY_MS = 50;

        public PowershellProgressInfoCalculator(int key, string name, List<Task> tasks) : base(key, name, tasks)
        {
        }

        public ProgressRecord GetPowershellProgressRecord()
        {
            var p = new ProgressRecord(ProgressActivityKey, ProgressActivityName, StatusDescription);
            p.PercentComplete = PercentComplete;
            p.SecondsRemaining = SecondsRemaining;
            return p;
        }

        private DateTime? lastProgressRecordOutput;

        public void WriteProgress(PSCmdlet cmdlet)
        {
            var now = DateTime.Now;
            if (!lastProgressRecordOutput.HasValue || now > lastProgressRecordOutput.Value.AddMilliseconds(PROGRESSBAR_UPDATE_FREQUENCY_MS) )
            {
                // only write progress every X milliseconds because it considerably slows down the process!
                lastProgressRecordOutput = now;
                cmdlet.WriteProgress(GetPowershellProgressRecord());
            }
        }
    }
}
