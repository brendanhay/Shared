using System;

namespace Infrastructure.Data
{
    public interface IUnitOfWork : IRepositoryFactory, IDisposable
    {
        void Commit();

        void Rollback();
    }
}
