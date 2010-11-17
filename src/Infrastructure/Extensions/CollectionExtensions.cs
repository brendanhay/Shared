using System.Collections.Generic;

namespace Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this IList<T> self, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable) {
                self.Add(item);
            }
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> self)
        {
            return self == null || self.Count < 1;
        }
    }
}
