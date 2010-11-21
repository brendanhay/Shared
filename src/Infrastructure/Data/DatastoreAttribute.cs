using System;

namespace Infrastructure.Data
{
    /// <summary>
    /// Signals to the data layer where classes annotated with 
    /// this attribute should be stored
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DatastoreAttribute : Attribute
    {
        public DatastoreAttribute(DatastoreType datastore)
        {
            Datastore = datastore;
        }

        public DatastoreType Datastore { get; private set; }
    }
}
