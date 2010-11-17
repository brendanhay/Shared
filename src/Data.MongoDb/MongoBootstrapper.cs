using Infrastructure;
using Infrastructure.Data;

namespace Data.MongoDb
{
    internal static class MongoBootstrapper
    {
        public static void Setup(IServiceLocator locator, string connection)
        {
            locator.Inject<IUnitOfWorkFactory>("mongoUnitOfWorkFactory",
                new MongoUnitOfWorkFactory(connection));
        }
    }
}