using System;
using Infrastructure;

namespace Data.NHibernate.Conventions
{
    internal static class NamingHelper
    {
        public static string GetPrefixedName(Type entityType)
        {
            // Work out the prefix based on the immediate namespace parent (ie. Namespace.<This>.Entity)
            var parts = (entityType.Namespace ?? "")
                .Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            // Create a singular prefix
            var prefix = Inflector.Singularize(parts.Length > 0 ? parts[parts.Length - 1] : "");

            var name = Inflector.Underscore(entityType.Name);

            // Compare the underscored name against the prefix and if they match just use the name
            return prefix.Equals(name.Split('_')[0], StringComparison.OrdinalIgnoreCase)
                ? name
                : prefix.ToLowerInvariant() + "_" + name;
        }
    }
}
