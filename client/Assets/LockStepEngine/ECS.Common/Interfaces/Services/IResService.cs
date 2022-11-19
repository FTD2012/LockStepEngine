namespace LockStepEngine
{
    public interface IResService : IService
    {
        string GetAssetPath(ushort assetId);
    }
}