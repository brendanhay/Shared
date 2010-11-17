using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Infrastructure;

namespace Data.NHibernate.Conventions
{
    internal sealed class HasManyToManyConvention : ManyToManyTableNameConvention, IHasManyToManyConvention
    {
        new public void Apply(IManyToManyCollectionInstance instance)
        {
            var entityType = instance.EntityType;
            var childName = instance.ChildType.Name;

            instance.LazyLoad();
            instance.BatchSize(5);
            instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
            instance.Cascade.All();
            instance.Key.Column(entityType.Name + "_id");
            instance.Relationship.Column(Inflector.Underscore(childName) + "_id");
            instance.Table(Inflector.Underscore(NamingHelper.GetPrefixedName(entityType) + childName, pluralize: true));
        }

        protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection,
            IManyToManyCollectionInspector otherSide)
        {
            return Inflector.Underscore(collection.EntityType.Name + "_" + otherSide.EntityType.Name);
        }

        protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
        {
            return Inflector.Underscore(collection.EntityType.Name + "_" + collection.ChildType.Name);
        }
    }
}
