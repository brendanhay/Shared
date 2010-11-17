using System.Reflection;
using Data.NHibernate.Conventions;
using FluentNHibernate.Cfg;

namespace Data.NHibernate
{
    internal static class FluentMappings
    {
        public static void AddMappings(Assembly mappings, MappingConfiguration config)
        {
            // Load the assembly where the entities live
            config.FluentMappings.AddFromAssembly(mappings);

            // Load naming, configuration conventions
            config.FluentMappings.Conventions.Setup(conventions => {
                conventions.Add<EnumConvention>();
                conventions.Add<ForeignKeyConvention>();
                conventions.Add<HasManyConvention>();
                conventions.Add<HasManyToManyConvention>();
                conventions.Add<IdConvention>();
                conventions.Add<PropertyNameConvention>();
                conventions.Add<ReferencesConvention>();
                conventions.Add<TableNameConvention>();
            });

            // Merge the mappings
            config.MergeMappings();
        }
    }
}
