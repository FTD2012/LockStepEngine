namespace LockStepEngine
{
    public interface IGameResourceService : IService
    {
        object LoadPrefab(int id);
    }
}