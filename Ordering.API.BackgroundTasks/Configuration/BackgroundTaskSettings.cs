using System;

namespace Ordering.API.BackgroundTasks.Configuration
{
    public class BackgroundTaskSettings
    {
        public string CheckUpdateTime { get; set; }
        public string ConnectionString { get; set; }
        public string PeriodTime { get; set; }
    }
}
