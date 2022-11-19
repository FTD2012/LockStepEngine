using LockStepEngine.Collision2D;

namespace LockStepEngine
{
    public class BaseComponent : IComponent
    {
        public BaseEntity baseEntity { get; private set; }
        public CTransform2D transform { get; private set; }

        public virtual void BindEntity(BaseEntity entity)
        {
            baseEntity = entity;
            transform = entity.transform;
        }

        public virtual void OnAwake()
        {
            
        }

        public virtual void OnStart()
        {
            
        }

        public virtual void OnUpdate(LFloat deltaTime)
        {
            
        }

        public virtual void OnDestroy()
        {
            
        }
    }
}