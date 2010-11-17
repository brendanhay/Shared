using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        private static readonly Dictionary<double, Func<TimeSpan, string>> _times;

        static DateTimeExtension()
        {
            _times = new Dictionary<double, Func<TimeSpan, string>> {
                { 0.75, x => "less than a minute" },
                { 1.5, x => "about a minute" },
                { 45, x => string.Format("{0} minutes", Convert.ToInt32(Math.Abs(x.TotalMinutes))) },
                { 90, x => "about an hour" },
                { 60 * 24, x => string.Format("about {0} hours", Convert.ToInt32(Math.Round(Math.Abs(x.TotalHours)))) },  
                { 60 * 48, x => "a day" },
                { 60 * 24 * 30, x => string.Format("{0} days", Convert.ToInt32(Math.Floor(Math.Abs(x.TotalDays)))) },
                { 60 * 24 * 60, x => "about a month" },
                { 60 * 24 * 365, x => string.Format("{0} months", Convert.ToInt32(Math.Floor(Math.Abs(x.TotalDays / 30)))) },
                { 60 * 24 * 365 * 2, x => "about a year" },
                { double.MaxValue, x => string.Format("{0:i} years", Convert.ToInt32(Math.Floor(Math.Abs(x.TotalDays / 365)))) }
            };
        }

        public static string ToRelativeDate(this DateTime self)
        {
            var difference = DateTime.Now.Subtract(self);
            var totalMinutes = Math.Abs(difference.TotalMinutes);
            var suffix = totalMinutes < 0.0 ? " from now" : " ago";
            var result = _times.First(n => totalMinutes < n.Key).Value(difference);

            return result + suffix;
        }

        public static string ToTimeStamp(this DateTime self)
        {
            return self.ToString("yyyyMMddHHmmssffff");
        }
    }
}
