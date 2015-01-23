#region

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using chnls.Service;

#endregion

namespace chnls.Utils
{
    internal class Scheduler
    {
        private static readonly List<TimerInstance> Timers = new List<TimerInstance>();
        private static readonly Dictionary<string, TimerInstance> Scheduled = new Dictionary<string, TimerInstance>();

        public static void RunIfNotScheduled(string key, string description, int delayMs, Action action)
        {
            if (String.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Key must not be null");
            }

            lock (Scheduled)
            {
                if (Scheduled.ContainsKey(key))
                {
                    return;
                }
                var timer = RunInternal(description, delayMs, action);
                timer.Key = key;
                Scheduled.Add(key, timer);
            }
        }

        public static void Run(string description, Action action, int delayMs)
        {
            RunInternal(description, delayMs, action);
        }

        private static TimerInstance RunInternal(string description, int delayMs, Action action)
        {
            TimerInstance instance = null;
            lock (Timers)
            {
                if (Timers.Count > 0)
                {
                    instance = Timers[0];
                    Timers.RemoveAt(0);
                }
            }
            if (null == instance)
            {
                instance = new TimerInstance();
            }
            if (delayMs < 1)
            {
                delayMs = 1;
            }
            instance.Action = action;
            instance.Description = description;
            instance.Timer.Interval = delayMs;
            instance.Timer.Enabled = true;
            return instance;
        }

        private class TimerInstance
        {
            public readonly Timer Timer = new Timer();
            internal Action Action;
            internal string Description;
            internal string Key;

            public TimerInstance()
            {
                Timer.Tick += timer_Tick;
            }

            private void timer_Tick(object sender, EventArgs e)
            {
                Timer.Enabled = false;
                if (!String.IsNullOrWhiteSpace(Key))
                {
                    lock (Scheduled)
                    {
                        if (Scheduled.ContainsKey(Key))
                        {
                            Scheduled.Remove(Key);
                            Key = null;
                        }
                    }
                }
                try
                {
                    Action();
                }
                catch (Exception ex)
                {
                    LoggingService.Error("Error executing scheduled action: " + Description, ex);
                }
                Action = null;
                lock (Timers)
                {
                    Timers.Add(this);
                }
            }
        }
    }
}