using System;
using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Extensions;

namespace Data
{
    internal sealed class UnitOfWorkCache : IDisposable
    {
        private readonly Func<DatastoreType, IUnitOfWork> _factory;
        private readonly IDictionary<DatastoreType, IUnitOfWork> _initialized
            = new Dictionary<DatastoreType, IUnitOfWork>();

        private readonly object _lock = new object();

        public UnitOfWorkCache(Func<DatastoreType, IUnitOfWork> factory)
        {
            _factory = factory;
        }

        public IUnitOfWork Get<T>()
        {
            IUnitOfWork unitOfWork;
            DatastoreAttribute attribute;

            // Set the default datastore
            var datastore = DatastoreType.Relational;

            // See if our type was marked for a specific datastore
            if (typeof(T).TryGetCustomAttribute(out attribute)) {
                datastore = attribute.Datastore;
            }

            lock (_lock) {
                // Lookup our internal cache of initialized unitofworks
                if (!_initialized.TryGetValue(datastore, out unitOfWork)) {
                    unitOfWork = _factory(datastore);

                    _initialized.Add(datastore, unitOfWork);
                }
            }

            return unitOfWork;
        }

        public void CommitAll()
        {
            foreach (var unitOfWork in _initialized.Values) {
                unitOfWork.Commit();
            }
        }

        public void RollbackAll()
        {
            Rollback(false);
        }

        private void Rollback(bool dispose)
        {
            foreach (var unitOfWork in _initialized.Values) {
                unitOfWork.Rollback();

                if (dispose) {
                    unitOfWork.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Rollback(true);
        }
    }
}
