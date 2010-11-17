using MongoDB;
using Infrastructure;
using Infrastructure.Data;

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

        #region IUnitOfWork

        public void Commit() { }

        public void Rollback() { }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _mongo.Disconnect();
            _mongo.Dispose();
        }

        #endregion

        #region IRepositoryFactory

        public IRepository<T> Repository<T>() where T : class
        {
            return new MongoRepository<T>(_mongo.GetDatabase("test"));
        }

        #endregion
    }
}
