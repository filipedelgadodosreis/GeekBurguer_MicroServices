using System;

namespace Ordering.API.BackgroundTasks.Configuration
{
    public class BackgroundTaskSettings
    {
        public int CheckUpdateTime { get; set; }
        public string ConnectionString { get; set; }
        public DateTime PeriodTime { get; set; }
    }
}
