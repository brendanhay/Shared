using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class IntExtension
    {
        private static readonly Dictionary<int, Func<double, string>> _days;

        static IntExtension()
        {
            _days = new Dictionary<int, Func<double, string>> {
                { 0, x => x.ToString() },
                { 1, x => string.Format("about {0} hours", Convert.ToInt32(Math.Round(Math.Abs(x)))) },  
                { 2, x => "a day" },
                { 30, x => string.Format("{0} days", Convert.ToInt32(Math.Floor(Math.Abs(x)))) },
                { 60, x => "about a month" },
                { 365, x => string.Format("{0} months", Convert.ToInt32(Math.Floor(Math.Abs(x / 30)))) },
                { 365 * 2, x => "about a year" },
                { int.MaxValue, x => string.Format("{0:i} years", Convert.ToInt32(Math.Floor(Math.Abs(x / 365)))) }
            };
        }

        public static string ToDays(this int self)
        {
            var abs = Math.Abs(self);
            var suffix = self < 0 ? " from now" : " ago";
            var result = _days.First(n => abs < n.Key).Value(abs);

            return result + suffix;
        }
    }
}
