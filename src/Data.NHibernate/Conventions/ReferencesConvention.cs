using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Infrastructure;

namespace Data.NHibernate.Conventions
{
    public class ReferencesConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Cascade.None();
            instance.Column(NamingHelper.GetPrefixedName(instance.Property.PropertyType) + "_id");
            instance.ForeignKey(string.Format("fk_{0}_{1}",
                Inflector.Underscore(instance.Property.PropertyType.Name),
                NamingHelper.GetPrefixedName(instance.EntityType)));
        }
    }
}
