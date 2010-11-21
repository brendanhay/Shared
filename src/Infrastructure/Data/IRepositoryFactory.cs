using Infrastructure.Domain;

namespace Infrastructure.Data
{
    public interface IRepositoryFactory
    {
        IRepository<T> Repository<T>() where T : class, IAggregate;
    }
}
