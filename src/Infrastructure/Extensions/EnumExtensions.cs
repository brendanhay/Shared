using System;
using System.Collections.Generic;

namespace Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static IList<string> GetFlagList<T>(this Enum flags)
        {
            var type = typeof(T);
            var list = new List<string>();

            foreach (var value in Enum.GetValues(type)) {
                if (Convert.ToInt32(value) != 0 && HasFlag(flags, Convert.ToInt32(value))) {
                    list.Add(Enum.GetName(type, value));
                }
            }

            return list;
        }

        private static bool HasFlag(Enum @enum, int check)
        {
            var against = Convert.ToInt32(@enum);

            return (against & check) == check;
        }
    }
}

