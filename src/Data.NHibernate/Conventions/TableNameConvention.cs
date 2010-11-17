using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using Infrastructure;

namespace Data.NHibernate.Conventions
{
    internal sealed class TableNameConvention : IClassConvention, IClassConventionAcceptance
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(string.Format("`{0}`",
                Inflector.Pluralize(NamingHelper.GetPrefixedName(instance.EntityType))));
        }

        public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
        {
            criteria.Expect(x => x.TableName, Is.Not.Set);
        }
    }
}
