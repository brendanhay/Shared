using Infrastructure.Data;
using Infrastructure.Domain;
using Norm;

namespace Data.NoRM
{
    internal sealed class NormUnitOfWork : IUnitOfWork
    {
        private readonly IMongo _mongo;

        public NormUnitOfWork(string connection)
        {
            _mongo = Mongo.Create(connection);
        }

        public void Commit() { }

        public void Rollback() { }

        public void Dispose()
        {
            _mongo.Dispose();
        }

        public IRepository<T> Repository<T>() where T : class, IAggregate
        {
            return new NormRepository<T>(_mongo.Database);
        }
    }
}
