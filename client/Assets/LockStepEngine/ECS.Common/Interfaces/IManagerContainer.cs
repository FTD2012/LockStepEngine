namespace LockStepEngine
{
    public interface IManagerContainer
    {
        T GetManager<T>() where T : BaseService;
    }
}