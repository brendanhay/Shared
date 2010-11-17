using Infrastructure;
using Infrastructure.Data;

namespace Data
{
    internal sealed class UnitOfWorkProxy : IUnitOfWork
    {
        private readonly UnitOfWorkCache _cache;

        public UnitOfWorkProxy(UnitOfWorkCache cache)
        {
            _cache = cache;
        }

        #region IUnitOfWork

        public void Commit()
        {
            _cache.CommitAll();
        }

        public void Rollback()
        {
            _cache.RollbackAll();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _cache.Dispose();
        }

        #endregion

        #region IRepositoryFactory
        
        public IRepository<T> Repository<T>() where T : class
        {
            return _cache.Get<T>().Repository<T>();
        }

        #endregion
    }
}
