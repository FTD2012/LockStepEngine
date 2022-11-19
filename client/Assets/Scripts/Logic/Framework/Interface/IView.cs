namespace LockStepEngine
{
    public interface IView
    {
        void BindEntity(BaseEntity e, BaseEntity oldEntity = null);
    }
}