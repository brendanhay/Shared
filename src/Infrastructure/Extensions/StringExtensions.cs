using System;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool AnyNullOrEmpty(params string[] strings)
        {
            return strings.Any(str => string.IsNullOrEmpty(str));
        }

        public static bool EqualsIgnoreCase(this string self, string other)
        {
            return self.Equals(other, StringComparison.OrdinalIgnoreCase);
        }
    }
}
