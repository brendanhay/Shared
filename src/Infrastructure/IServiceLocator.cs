using System;
using System.Collections.Generic;

namespace Infrastructure
{
    public interface IServiceLocator : IServiceProvider
    {
        void Add<TInterface>(Type type, bool transient);
        void Add<TInterface>(Type type, string name, bool transient);

        void Add<TInterface, TConcrete>(bool transient) where TConcrete : TInterface;
        void Add<TInterface, TConcrete>(string name, bool transient) where TConcrete : TInterface;

        void Inject<T>(T instance);
        void Inject<T>(string name, T instance);

        object Get(Type type);
        T Get<T>();

        object Get(Type type, string name);
        T Get<T>(string name);

        IEnumerable<object> GetAll(Type type);
        IEnumerable<T> GetAll<T>();
    }
}