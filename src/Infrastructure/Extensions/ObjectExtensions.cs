using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Infrastructure.Extensions
{
    public static class ObjectExtensions
    {
        public static T DeepClone<T>(this T self)
        {
            using (var stream = new MemoryStream()) {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, self);

                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }

        public static void Extend(this object source, object destination, int depth = 0)
        {
            var type = source.GetType();

            while (type != null) {
                UpdateForType(type, source, destination);

                type = depth > 0 ? type.BaseType : null;

                depth--;
            }
        }

        private static void UpdateForType(this Type type, object source, object destination)
        {
            var fields = type.GetFields(BindingFlags.NonPublic
                | BindingFlags.Public | BindingFlags.Instance);

            foreach (var fi in fields) {
                fi.SetValue(destination, fi.GetValue(source));
            }
        }
    }
}
