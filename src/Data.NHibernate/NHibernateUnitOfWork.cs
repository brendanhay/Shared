using System;
using System.Data;
using NHibernate;
using Infrastructure;
using Infrastructure.Data;

namespace Data.NHibernate
{
    internal sealed class NHibernateUnitOfWork : IUnitOfWork
    {
        private readonly ISession _session;

        private ITransaction _transaction;

        internal NHibernateUnitOfWork(ISessionFactory factory)
        {
            _session = factory.OpenSession();
            _session.FlushMode = FlushMode.Auto;
            _transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        #region IUnitOfWork

        public void Commit()
        {
            if (!_transaction.IsActive) {
                throw new InvalidOperationException("No active transaction.");
            }

            _transaction.Commit();
            _transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Rollback()
        {
            if (_transaction.IsActive) {
                _transaction.Rollback();
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _session.Close();
        }

        #endregion

        #region IRepositoryFactory

        public IRepository<T> Repository<T>() where T : class
        {
            return new Repository<T>(_session);
        }

        #endregion
    }
}
