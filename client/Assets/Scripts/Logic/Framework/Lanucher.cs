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
        private TimeMachineContainer timeMachineContainer;
        private OneThreadSynchronizationContext syncContext;
        private SimulatorService simulatorService = new SimulatorService();
        

        public void OnAwake(ServiceContainer _serviceContainer)
        {
            if (_serviceContainer == null)
            {
                GLog.Error("ServiceContainer is invalid!");
                return;
            }
            serviceContainer = _serviceContainer;;
            
            if (instance != null)
            {
                GLog.Error("LifeCycle Error: Awake more than once!");
                return;;
            }
            instance = this;
            
            eventRegisterService = new EventRegisterService();
            managerContainer = new ManagerContainer();
            timeMachineContainer = new TimeMachineContainer();
            syncContext = new OneThreadSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(syncContext);
            UtilSystem.StartService();

            foreach (var service in serviceContainer.GetAll())
            {
                if (service is BaseService baseService)
                {
                    managerContainer.RegisterManager(baseService);
                }
                
                if (service is ITimeMachine timeMachine)
                {
                    timeMachineContainer.RegisterTimeMachine(timeMachine);
                }
            }

            serviceContainer.Register(timeMachineContainer);
            serviceContainer.Register(eventRegisterService);
        }

        public void OnStart()
        {
            foreach (var service in managerContainer.serviceList)
            {
                service.InitReference(serviceContainer, managerContainer);
            }

            foreach (var service in managerContainer.serviceList)
            {
                eventRegisterService.RegisterEvent<EventType, EventHandler>("OnEvent_", "OnEvent_".Length, EventHelper.AddListener, service);
            }

            foreach (var service in managerContainer.serviceList)
            {
                service.OnAwake(serviceContainer);
            }
            
            
        }

        public void _DoAwake(ServiceContainer serviceContainer)
        {
            
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