using System;

namespace Cron.NET
{
    public class ScheduleData
    {
        public CronJob cron { get; set; }
        public Action action { get; set; }
    }

}