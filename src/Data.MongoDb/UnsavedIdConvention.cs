using System;
using MongoDB.Configuration.Mapping.Conventions;

namespace Data.MongoDb
{
    internal sealed class UnsavedIdConvention : IIdUnsavedValueConvention
    {
        public object GetUnsavedValue(Type type)
        {
            return null;
        }
    }
}
