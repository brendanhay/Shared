namespace Infrastructure.Data
{
    public interface IUnitOfWorkFactory
    {
        DatastoreType Datastore { get; }

        IUnitOfWork Create();

#if DEBUG
        void Rebuild();
#endif
    }
}
