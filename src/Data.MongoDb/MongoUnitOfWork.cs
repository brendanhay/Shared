using Infrastructure.Data;
using Infrastructure.Domain;
using MongoDB;
using MongoDB.Configuration;

namespace Data.MongoDb
{
    internal sealed class MongoUnitOfWork : IUnitOfWork
    {
        private readonly IMongo _mongo;

        public MongoUnitOfWork(string connection)
        {
            var builder = new MongoConfigurationBuilder();

            builder.ConnectionString(connection);
            builder.Mapping(m => m.DefaultProfile(profile => profile
                .UseIdUnsavedValueConvention(new UnsavedIdConvention())));

            _mongo = new Mongo(builder.BuildConfiguration());
            _mongo.Connect();
        }

        public void Commit() { }

        public void Rollback() { }

        public void Dispose()
        {
            _mongo.Disconnect();
            _mongo.Dispose();
        }

        public IRepository<T> Repository<T>() where T : class, IAggregate
        {
            return new MongoRepository<T>(_mongo.GetDatabase("test"));
        }
    }
}
