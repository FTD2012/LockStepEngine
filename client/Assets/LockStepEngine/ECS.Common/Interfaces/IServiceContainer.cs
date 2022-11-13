namespace LockStepEngine
{
    public interface IServiceContainer
    {
        T Get<T>() where T : IService;
        IService[] GetAll();
    }
}