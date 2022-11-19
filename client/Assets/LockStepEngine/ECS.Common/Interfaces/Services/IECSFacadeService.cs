namespace LockStepEngine
{
    public interface IECSFacadeService : IService
    {
        IContext CreateContext();
    }
}