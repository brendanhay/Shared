using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Infrastructure;

namespace Data.NHibernate.Conventions
{
    internal sealed class HasManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.LazyLoad();
            instance.BatchSize(5);
            instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
            instance.Cascade.All();
            instance.Key.Column(Inflector.Underscore(instance.EntityType.Name) + "_id");
            instance.Key.ForeignKey(string.Format("fk_{0}_{1}",
                NamingHelper.GetPrefixedName(instance.EntityType),
                Inflector.Underscore(instance.ChildType.Name).ToLower()));
        }
    }
}

