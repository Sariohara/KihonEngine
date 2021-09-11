using System;
using System.Collections.Generic;

namespace KihonEngine.Services
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly Dictionary<string, object> _services;

        public ServiceLocator()
        {
            _services = new Dictionary<string, object>();
        }

        public void Register<TInterface>(TInterface implementation)
        {
            var key = typeof(TInterface).FullName;
            if (!_services.ContainsKey(key))
            {
                _services.Add(key, implementation);
            }
            else
            {
                throw new InvalidOperationException($"type <{key}> is already registered");
            }
        }

        public TInterface Get<TInterface>()
        {
            var key = typeof(TInterface).FullName;
            if (_services.ContainsKey(key))
            {
                return (TInterface)_services[key];
            }

            throw new InvalidOperationException($"type <{key}> is not registered");
        }
    }
}
