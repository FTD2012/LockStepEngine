namespace LockStepEngine
{
    public interface ILifeCycle
    {
        void OnAwake(ServiceContainer serviceContainer);
        void OnStart();
        void OnApplicationQuit();
        void OnDestroy();
    }
}