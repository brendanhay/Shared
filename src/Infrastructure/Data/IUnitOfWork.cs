using System;

namespace Infrastructure
{
    public interface IUnitOfWork : IRepositoryFactory, IDisposable
    {
        void Commit();

        void Rollback();
    }
}
