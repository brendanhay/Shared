using Infrastructure;
using Infrastructure.Data;

namespace Data
{
    public static class DataBootstrapper
    {
        public static void Setup(IServiceLocator locator)
        {
            locator.Add<IUnitOfWorkFactoryProxy, UnitOfWorkFactoryProxy>(false);
        }
    }
}
