using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Infrastructure
{
    public sealed class UnityContainerAdapter : IServiceLocator
    {
        private readonly IUnityContainer _container;

        public UnityContainerAdapter()
        {
            _container = new UnityContainer();
        }

        #region IServiceProvider Members

        public object GetService(Type type)
        {
            return Get(type);
        }

        #endregion

        private static LifetimeManager GetLifetimeManager(bool transient)
        {
            return transient ? new TransientLifetimeManager() : default(LifetimeManager);
        }

        #region IServiceLocator Members

        public void Add<TInterface>(Type type, bool transient = false)
        {
            _container.RegisterType(typeof(TInterface), type, GetLifetimeManager(transient));
        }

        public void Add<TInterface>(Type type, string name, bool transient = false)
        {
            _container.RegisterType(typeof(TInterface), type, name, GetLifetimeManager(transient));
        }

        public void Add<TInterface, TConcrete>(bool transient = false) where TConcrete : TInterface
        {
            _container.RegisterType<TInterface, TConcrete>(GetLifetimeManager(transient));
        }

        public void Add<TInterface, TConcrete>(string name, bool transient = false) where TConcrete : TInterface
        {
            _container.RegisterType<TInterface, TConcrete>(name, GetLifetimeManager(transient));
        }

        public void Inject<T>(T instance)
        {
            _container.RegisterInstance(instance);
        }

        public void Inject<T>(string name, T instance)
        {
            _container.RegisterInstance(name, instance);
        }

        public object Get(Type type)
        {
            return _container.Resolve(type);
        }

        public T Get<T>()
        {
            return _container.Resolve<T>();
        }

        public object Get(Type type, string name)
        {
            return _container.Resolve(type, name);
        }

        public T Get<T>(string name)
        {
            return _container.Resolve<T>(name);
        }

        public IEnumerable<object> GetAll(Type type)
        {
            return _container.ResolveAll(type);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _container.ResolveAll<T>();
        }

        #endregion
    }
}
