using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Infrastructure.Data;

namespace Data
{
    internal sealed class UnitOfWorkFactoryProxy : IUnitOfWorkFactory
    {
        private readonly IDictionary<DatastoreType, IUnitOfWorkFactory> _factories;

        public UnitOfWorkFactoryProxy(IEnumerable<IUnitOfWorkFactory> factories)
        {
            _factories = factories.ToDictionary(factory => factory.Datastore, factory => factory);

            foreach (var value in (DatastoreType[])Enum.GetValues(typeof(DatastoreType))) {
                if (_factories.ContainsKey(value)) {
                    continue;
                }

                var message = string.Format("UnitOfWorkFactory for DatastoreType '{0}' has not been registered.",
                    value);

                throw new ArgumentException(message);
            }
        }

        public DatastoreType Datastore
        {
            get { throw new NotImplementedException(); }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope",
            Justification = "UnitOfWork lifecycle is managed by the caller")]
        public IUnitOfWork Create()
        {
            var cache = new UnitOfWorkCache(datastore => GetFactory(datastore).Create());

            return new UnitOfWorkProxy(cache);
        }

#if DEBUG
        void IUnitOfWorkFactory.Rebuild()
        {
            foreach (var datastore in _factories.Keys) {
                GetFactory(datastore).Rebuild();
            }
        }
#endif

        private IUnitOfWorkFactory GetFactory(DatastoreType datastore)
        {
            IUnitOfWorkFactory factory;

            if (!_factories.TryGetValue(datastore, out factory)) {
                throw new NotSupportedException("Specified datastore is not supported.");
            }

            return factory;
        }
    }
}
