namespace LockStep.Game
{
    public interface IServiceContainer
    {
        T Get<T>() where T : IService;
        IService[] GetAll();
    }
}