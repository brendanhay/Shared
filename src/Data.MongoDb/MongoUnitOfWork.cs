using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Domain;
using MongoDB;

namespace Data.MongoDb
{
    internal sealed class MongoUnitOfWork : IUnitOfWork
    {
        private readonly IMongo _mongo;

        public MongoUnitOfWork(IMongo mongo)
        {
            _mongo = mongo;
            _mongo.Connect();
        }

        public void Commit() { }

        public void Rollback() { }

        public void Dispose()
        {
            _mongo.Disconnect();
            _mongo.Dispose();
        }

        public IRepository<T> Repository<T>() where T : class, IAggregate
        {
            return new MongoRepository<T>(_mongo.GetDatabase("test"));
        }
    }
}
