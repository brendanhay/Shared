using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Tuple<T, Type>> GetTypesWithAttribute<T>(this Assembly self)
            where T : Attribute
        {
            return self.GetTypesWithAttribute<T>(attribute => true);
        }

        public static IEnumerable<Tuple<T, Type>> GetTypesWithAttribute<T>(this Assembly self,
            Func<T, bool> predicate) where T : Attribute
        {
            foreach (var type in self.GetTypes()) {
                var attributes = type.GetCustomAttributes(typeof(T), true);

                if (attributes.Length > 0) {
                    var attribute = attributes.Cast<T>().FirstOrDefault();

                    if (attribute != null && predicate(attribute)) {
                        yield return Tuple.Create(attribute, type);
                    }
                }
            }
        }

        public static string CodeBaseDirectory(this Assembly self)
        {
            var index = self.CodeBase.LastIndexOf('/');

            return self.CodeBase.Remove(index).Replace("file:///", "");
        }
    }
}
