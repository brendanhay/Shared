using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Infrastructure;

namespace Data.NHibernate.Conventions
{
    internal sealed class ColumnNameConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            instance.Column(string.Format("`{0}`", Inflector.Underscore(instance.Name)));
        }
    }
}
