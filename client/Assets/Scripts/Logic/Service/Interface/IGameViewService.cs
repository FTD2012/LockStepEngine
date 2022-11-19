namespace LockStepEngine
{
    public interface IGameViewService : IService
    {
        void BindView(BaseEntity entity, BaseEntity oldEntity = null);
        void UnBindView(BaseEntity entity);
    }
}