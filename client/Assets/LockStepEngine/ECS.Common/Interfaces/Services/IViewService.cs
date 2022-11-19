namespace LockStepEngine
{
    public interface IViewService : IService
    {
        void BindView(IEntity entity, ushort assetId, LVector2 createPos, int deg = 0);
        void DelteView(uint entityId);
        void RebindView(IEntity entity);
        void RebindAllEntityes();
    }
}