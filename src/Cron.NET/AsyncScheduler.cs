using System;
using System.Collections.Generic;
using System.Linq;

namespace Cron.NET
{
    // Non-Blocking Scheduler
    public class AsyncScheduler
    {
        // https://unix.stackexchange.com/questions/82557/how-is-cron-scheduling-actually-implemented-and-makes-sure-scripts-run-on-time

        public bool AutoStartOnSchedule { get; set; }

        public AsyncScheduler(bool autoStartOnSchedule = true)
        {
            AutoStartOnSchedule = autoStartOnSchedule;
        }

        private LinkedList<ScheduleData> _data { get; set; } = new LinkedList<ScheduleData>();

        public void Schedule(CronJob cron, Action action)
        {

        }

        public void Start()
        {

        }

        public void Stop()
        {

        }

        private void Work()
        {
            // ScheduleData d = _data.First();
            // _data.RemoveFirst();

            // var interval = d.cron.GetNextDate() - DateTime.Now;

            // Thread.Sleep((int)interval.TotalMilliseconds);

            // if (DateTime.Now >= d.cron.GetNextDate())
            // {
            //     //TODO: Run Action

            // }
            // else
            // {
            //     Work();
            // }

        }

        private void InsertToList(ScheduleData data)
        {
            //DateTime next = data.cron.GetNextDate();

            //var new_node = new LinkedListNode<ScheduleData>(data);

            //var node = _data.First;
            //if (node is null)
            //{
            //    _data.AddFirst(new_node);
            //}
            //else
            //{
            //    while (node.Value.cron.GetNextDate() < next)
            //    {
            //        var next_node = node.Next;
            //        if (next_node is null)
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            node = next_node;
            //        }
            //    }

            //    _data.AddAfter(node, new_node);
            //}

        }

    }
}