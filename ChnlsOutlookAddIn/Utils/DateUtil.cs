#region

using System;
using System.Globalization;

#endregion

namespace chnls.Utils
{
    public class DateUtil
    {
        public static long NowMs
        {
            get { return (long) (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds; }
        }

        public static DateTime DateTimeFrom1970(long milliseconds)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(milliseconds);
        }

        public static DateTime ConvertFromUnixTimestamp(long timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        public static long ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = date.ToUniversalTime() - origin;
            return (long) Math.Floor(diff.TotalSeconds);
        }

        public static string RelativeDateString(long date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var prevDate = epoch.AddMilliseconds(date);
            var now = DateTime.UtcNow.Ticks;
            var prevTicks = prevDate.Ticks;
            var ts = new TimeSpan(now - prevTicks);
            var delta = Math.Abs(ts.TotalSeconds);

            const int second = 1;
            const int minute = 60*second;
            const int hour = 60*minute;
            const int day = 24*hour;

            if (delta < 0)
            {
                return "not yet";
            }
            if (delta < 1*minute)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2*minute)
            {
                return "a minute ago";
            }
            if (delta < 45*minute)
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 90*minute)
            {
                return "an hour ago";
            }
            if (delta < 24*hour)
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 48*hour)
            {
                return "yesterday";
            }
            if (delta < 14*day)
            {
                return prevDate.ToString("g", DateTimeFormatInfo.InvariantInfo);
            }
            return prevDate.ToString("d", DateTimeFormatInfo.InvariantInfo);
        }
    }
}