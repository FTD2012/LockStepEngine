namespace LockStepEngine
{
    public interface IEffectServicr  : IService
    {
        void CreateEffect(int assetId, LVector2 pos);
        void DestroyEffect(object node);
    }
}