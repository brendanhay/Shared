using System.Reflection;
using Infrastructure;
using Infrastructure.Data;

namespace Data.NHibernate
{
    internal static class NHibernateBootstrapper
    {
        public static void Setup(IServiceLocator locator, Assembly mappings, string connection)
        {
            locator.Inject<IUnitOfWorkFactory>("nhibernateUnitOfWorkFactory",
                new NHibernateUnitOfWorkFactory(mappings, connection));
        }
    }
}