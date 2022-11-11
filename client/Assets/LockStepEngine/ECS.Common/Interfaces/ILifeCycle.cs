namespace LockStep.Game
{
    public interface ILifeCycle
    {
        void OnAwake(IServiceContainer serviceContainer);
        void OnStart();
        void OnApplicationQuit();
        void OnDestroy();
    }
}