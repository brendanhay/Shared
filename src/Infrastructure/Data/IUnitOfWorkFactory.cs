namespace Infrastructure.Data
{
    // TODO: Get rid of the rebuilds, since 
    // they're only currently there for quick protoyping

    public interface IUnitOfWorkFactoryProxy
    {
        IUnitOfWork Create();

#if DEBUG
        void Rebuild(DatastoreType datastore);
#endif
    }

    public interface IUnitOfWorkFactory
    {
        DatastoreType Datastore { get; }

        IUnitOfWork Create();

#if DEBUG
        void Rebuild();
#endif
    }
}
