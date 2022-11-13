using System.Collections.Generic;

namespace LockStepEngine.Game
{
    public class ManagerContainer : IManagerContainer
    {
        public List<BaseService> serviceList = new List<BaseService>();
        private Dictionary<string, BaseService> serviceDic = new Dictionary<string, BaseService>();

        public void RegisterManager(BaseService service)
        {
            var name = service.GetType().Name;
            if (serviceDic.ContainsKey(name))
            {
                GLog.Error($"Duplicate Register manager {name} type:{service?.GetType().ToString() ?? ""}");
                return;
            }
            
            serviceDic.Add(name, service);
            serviceList.Add(service);
        }

        public T GetManager<T>() where T : BaseService
        {
            if (serviceDic.TryGetValue(typeof(T).Name, out var service))
            {
                return service as T;
            }

            return null;
        }

        public List<BaseService> GetServiceList()
        {
            return serviceList;
        }
    }
}