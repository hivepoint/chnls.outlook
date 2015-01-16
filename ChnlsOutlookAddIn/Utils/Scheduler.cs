#region

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using chnls.Service;
using chnls.Services;

#endregion

namespace chnls.Utils
{
    internal class Scheduler
    {
        private static readonly List<TimerInstance> Timers = new List<TimerInstance>();

        public static void Run(string description, Action action, int delayMs)
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
        }

        private class TimerInstance
        {
            public readonly Timer Timer = new Timer();
            internal Action Action;
            internal String Description;

            public TimerInstance()
            {
                Timer.Tick += timer_Tick;
            }

            private void timer_Tick(object sender, EventArgs e)
            {
                Timer.Enabled = false;
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