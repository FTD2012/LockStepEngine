using System;
using System.Collections.Generic;
using System.Linq;

namespace LockStep.Game
{
    // TODO: ljm >>> refactor to manager
    public abstract class ServiceContainer : IServiceContainer
    {
        private Dictionary<Type, IService> serviceDic = new Dictionary<Type, IService>();

        public void Register(IService service, bool overwrite = true)
        {
            var interfaceTypes = service.GetType().FindInterfaces((type, criteria) => type.GetInterfaces().Any(t => t == typeof(IService)), service).ToArray();

            foreach (var type in interfaceTypes)
            {
                if (!serviceDic.ContainsKey(type))
                {
                    serviceDic.Add(type, service);
                }
                else if (overwrite)
                {
                    serviceDic[type] = service;
                }
            }
        }
        
        public T Get<T>() where T : IService
        {
            var type = typeof(T);
            if (!serviceDic.ContainsKey(type))
            {
                return default(T);
            }

            return (T) serviceDic[type];
        }

        public IService[] GetAll()
        {
            return serviceDic.Values.ToArray();
        }
    }
}