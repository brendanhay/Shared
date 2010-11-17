using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        public static IList<TAttribute> GetCustomAttributes<TAttribute>(this Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), false)
                .Select(attribute => (TAttribute)attribute).ToList();

            return attributes;
        }

        public static bool HasCustomAttribute<TAttribute>(this Type type)
        {
            return type.GetCustomAttributes(typeof(TAttribute), false).Length > 0;
        }

        public static bool TryGetCustomAttribute<TAttribute>(this Type type, out TAttribute attribute)
        {
            var attributes = type.GetCustomAttributes(typeof(TAttribute), false);

            if (attributes.Length > 0) {
                attribute = (TAttribute)attributes[0];

                return true;
            }

            attribute = default(TAttribute);

            return false;
        }


        //public static void Extend(this Type self, object destination, object source)
        //{
        //    var left = GetAllFields(destination.GetType());
        //    var right = GetAllFields(source.GetType());

        //    var type = self;

        //    while (type != null) {

        //        foreach (var pair in GetAllFields(self)) {
        //            var name = pair.Key;

        //            if (!left.ContainsKey(name) || !right.ContainsKey(name)) {
        //                continue;
        //            }

        //            var field = pair.Value;

        //            field.SetValue(self, field.GetValue(source));
        //        }

        //        type = type.BaseType;
        //    }
        //}

        //private static IDictionary<string, FieldInfo> GetAllFields<T>()
        //{
        //    return GetAllFields(typeof(T));
        //}

        //private static IDictionary<string, FieldInfo> GetAllFields(IReflect type)
        //{
        //    const BindingFlags flags = BindingFlags.NonPublic
        //        | BindingFlags.Public | BindingFlags.Instance;

        //    return type.GetFields(flags).ToDictionary(field => field.Name, field => field);
        //}
    }
}
