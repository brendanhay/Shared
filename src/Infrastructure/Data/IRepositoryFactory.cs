using Infrastructure.Data;

namespace Infrastructure
{
    public interface IRepositoryFactory
    {
        IRepository<T> Repository<T>() where T : class;
    }
}
