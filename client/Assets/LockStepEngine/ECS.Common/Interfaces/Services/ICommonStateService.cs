namespace LockStepEngine
{
    public interface ICommonStateService : IService
    {
        int Tick { get; }
        
    }
}