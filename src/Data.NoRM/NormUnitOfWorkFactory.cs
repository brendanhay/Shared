using System.Reflection;
using Infrastructure.Data;
using Norm.Configuration;

namespace Data.NoRM
{
    internal sealed class MongoUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly string _connection;
        private readonly Assembly _mappings;


        public MongoUnitOfWorkFactory(string connection, Assembly mappings)
        {
            _connection = connection;
            _mappings = mappings;

            MongoConfiguration.Initialize(config => config.For(a => a.)

            //var builder = new MongoConfigurationBuilder();

            //builder.ConnectionString(connection);
            //builder.Mapping(m => m.DefaultProfile(profile => profile
            //    .UseIdUnsavedValueConvention(new UnsavedIdConvention())));
            //builder.Mapping(m => m.DefaultProfile(profile => profile
            //    .AliasesAre(alias => alias.)

            //_mongo = new Mongo(builder.BuildConfiguration());
        }

        public DatastoreType Datastore { get { return DatastoreType.Document; } }

        public IUnitOfWork Create()
        {
            return new NormUnitOfWork(_connection);
        }

#if DEBUG
        void IUnitOfWorkFactory.Rebuild()
        {
            //            var mongo = Mongo.Create(_connection);

        }
#endif
    }
}