using System.ComponentModel.Design;
using System.Threading;
using LockStep.Network;
using LockStep.Util;
using UnityEngine;

namespace LockStep.Game
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
            syncContext = new OneThreadSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(syncContext);
            UtilSystem.StartService();

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