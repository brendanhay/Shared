using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Infrastructure;

namespace Data.NHibernate.Conventions
{
    public class PropertyNameConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.Column(Inflector.Underscore(instance.Property.Name));
        }
    }
}
