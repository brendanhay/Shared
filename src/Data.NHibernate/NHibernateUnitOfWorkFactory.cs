using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure;
using Infrastructure.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Data.NHibernate
{
    internal sealed class NHibernateUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly Assembly _mappings;
        private readonly string _connection;

        public NHibernateUnitOfWorkFactory(Assembly mappings, string connection)
        {
            _mappings = mappings;
            _connection = connection;
            _sessionFactory = Configure().BuildSessionFactory();
        }

        public DatastoreType Datastore { get { return DatastoreType.Relational; } }

        public IUnitOfWork Create()
        {
            return new NHibernateUnitOfWork(_sessionFactory);
        }

#if DEBUG
        void IUnitOfWorkFactory.Rebuild()
        {
            var configuration = Configure();
            var schema = new SchemaExport(configuration);

            schema.Drop(false, true);
            schema.Create(false, true);
        }
#endif

        internal Configuration Configure()
        {
            var listeners = new[] { new AuditEventListener() };

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(_connection))
                .Mappings(config => FluentMappings.AddMappings(_mappings, config))
                .ExposeConfiguration(config => config.EventListeners.PreUpdateEventListeners = listeners)
                .ExposeConfiguration(config => config.EventListeners.PreInsertEventListeners = listeners)
                .BuildConfiguration();
        }
    }
}