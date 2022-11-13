using System.ComponentModel.Design;
using System.Threading;
using UnityEngine;

namespace LockStepEngine.Game
{
    public class Lanucher : ILifeCycle
    {
        private static Lanucher instance;
        public static Lanucher Instance
        {
            get
            {
                return instance;
            }
        }

        private ServiceContainer serviceContainer;
        private EventRegisterService eventRegisterService;
        private ManagerContainer managerContainer;
        private OneThreadSynchronizationContext syncContext;


        public void OnAwake(IServiceContainer container)
        {
            if (instance != null)
            {
                Debug.LogError("LifeCycle Error: Awake more than once!!");
                return;;
            }
            instance = this;
            
            serviceContainer = container as ServiceContainer;;
            eventRegisterService = new EventRegisterService();
            managerContainer = new ManagerContainer();
            syncContext = new OneThreadSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(syncContext);
            UtilSystem.StartService();

        }

        public void OnAwake(ServiceContainer serviceContainer)
        {
            throw new System.NotImplementedException();
        }

        public void OnStart()
        {
            throw new System.NotImplementedException();
        }

        public void OnApplicationQuit()
        {
            throw new System.NotImplementedException();
        }

        public void OnDestroy()
        {
            throw new System.NotImplementedException();
        }
    }

}