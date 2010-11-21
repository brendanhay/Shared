using System;

namespace Infrastructure.Domain
{
    public interface IValueObject : IComparable, IEquatable<IValueObject> { }

    public abstract class ValueObject : IValueObject
    {
        public virtual int PersistedId { get; protected internal set; }

        public virtual int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public virtual bool Equals(IValueObject other)
        {
            throw new NotImplementedException();
        }
    }
}
