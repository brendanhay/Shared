using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Data.NHibernate.Conventions
{
    internal sealed class IdConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column("id");
            instance.GeneratedBy.Identity();
        }
    }
}