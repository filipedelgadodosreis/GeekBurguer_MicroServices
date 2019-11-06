﻿using System;

namespace Payment.BackgroundTasks.Configuration
{
    public class BackgroundTaskSettings
    {
        public int CheckUpdateTime { get; set; }
        public string EventBusConnection { get; set; }
        public string ConnectionString { get; set; }
        public DateTime PeriodTime { get; set; }
    }
}
