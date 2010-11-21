using System;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Extensions;
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

        public NHibernateUnitOfWorkFactory(string connection, Assembly mappings)
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

            schema.SetOutputFile(GetSchemaFileName());

            schema.Drop(false, true);
            schema.Create(true, true);
        }
#endif

        private Configuration Configure()
        {
            var listeners = new[] { new AuditEventListener() };

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(_connection))
                .Mappings(config => FluentMappings.AddMappings(_mappings, config))
                .ExposeConfiguration(config => config.EventListeners.PreUpdateEventListeners = listeners)
                .ExposeConfiguration(config => config.EventListeners.PreInsertEventListeners = listeners)
                .BuildConfiguration();
        }

        private static string GetSchemaFileName()
        {
            var current = Assembly.GetExecutingAssembly().CodeBaseDirectory();
            var parent = Path.GetDirectoryName(current) ?? "";
            var directory = Path.Combine(parent, "data", "nhibernate");

            Directory.CreateDirectory(directory);

            return Path.Combine(directory, DateTime.Now.ToTimeStamp() + ".sql");
        }
    }
}