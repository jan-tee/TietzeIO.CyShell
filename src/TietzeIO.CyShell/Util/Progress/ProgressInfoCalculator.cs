using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Util.Progress
{
    public class ProgressInfoCalculator
    {
        public string ProgressActivityName { get; set; }
        public int ProgressActivityKey { get; set; } = 0;
        public string StatusDescription { get; private set; }
        public int PercentComplete { get; private set; } = 0;
        public int SecondsRemaining { get; private set; } = 0;

        public double rate { get; private set; }
        public double total_rate { get; private set; }
        public long total { get; private set; }

        private DateTime? start = null;
        private DateTime? _nextStatusCalculatedAt = null;
        private DateTime? _lastStatusCalculatedAt = null;
        private long _lastCompleted = 0;

        public bool StatusIsDirty
        {
            get => (!_nextStatusCalculatedAt.HasValue) || (_nextStatusCalculatedAt <= DateTime.Now);
        }

        protected List<Task> _tasks;

        public ProgressInfoCalculator(int key, string name, List<Task> tasks)
        {
            ProgressActivityKey = key;
            ProgressActivityName = name;
            _tasks = tasks;
            start = DateTime.Now;
        }

        private long _queueLength;
        private long _completed;
        private long _cancelled;
        private long _faulted;

        public void CalculateProgress(bool force = false)
        {
            if (!(force || StatusIsDirty)) return;

            var currentTime = DateTime.Now;
            _nextStatusCalculatedAt = currentTime.AddSeconds(PowershellModuleConstants.UPDATE_PROGRESS_INTERVAL);

            total = _tasks.Count;
            _completed = _tasks.Count(t => t.Status == TaskStatus.RanToCompletion);
            _cancelled = _tasks.Count(t => t.Status == TaskStatus.Canceled);
            _faulted = _tasks.Count(t => t.Status == TaskStatus.Faulted);
            _queueLength = total - (_completed + _cancelled + _faulted);

            if (_lastStatusCalculatedAt.HasValue)
            {
                rate = (_completed - _lastCompleted) / (currentTime - _lastStatusCalculatedAt.Value).TotalSeconds;
            }
            if (start.HasValue)
            {
                total_rate = _completed / (currentTime - start.Value).TotalSeconds;
            }
            _lastStatusCalculatedAt = currentTime;
            _lastCompleted = _completed;

            if (total > 0)
            {
                PercentComplete = (int)(100 * _completed / total);
            }
            if (total_rate > 0)
            {
                SecondsRemaining = (int)((total - _completed) / total_rate);
            }

            StatusDescription = $"Tasks: Total={total} Queued={_queueLength} Completed={_completed} Cancelled={_cancelled} Faulted={_faulted} CurrentRate={rate:F1}/TotalRate={total_rate:F1} items/s";
        }
    }
}
