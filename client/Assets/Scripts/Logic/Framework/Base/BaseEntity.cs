using System.Collections.Generic;
using LockStepEngine.Collision2D;

namespace LockStepEngine
{
    public class BaseEntity : BaseLiftCycle, IEntity, ILPTriggerEventHandler
    {
        public int EntityId;
        public int PrefabId;
        public CTransform2D transform = new CTransform2D();
        protected List<BaseComponent> componentList = new List<BaseComponent>();
        [NoBackup] public object engineTransform;
        
        [ReRefBackup] public IGameStateService GameStateService { get; set; }
        [ReRefBackup] public IServiceContainer ServiceContainer { get; set; }
        [ReRefBackup] public IDebugService DebugService { get; set; }
        [ReRefBackup] public IEntityView EntityView { get; set; }

        public T GetService<T>() where T : IService
        {
            return ServiceContainer.Get<T>();
        }

        public void OnBindRef()
        {
            BindRef();
        }

        public virtual void OnRollbackDestroy()
        {
            EntityView?.OnRollbackDestroy();
            EntityView = null;
            engineTransform = null;
        }

        protected virtual void BindRef()
        {
            componentList?.Clear();
        }

        private void RegisterComponent(BaseComponent component)
        {
            if (componentList == null)
            {
                componentList = new List<BaseComponent>();
            }

            componentList.Add(component);
            component.BindEntity(this);
        }

        public override void OnAwake()
        {
            foreach (var component in componentList)
            {
                component.OnAwake();
            }
        }
        
        public override void OnStart()
        {
            foreach (var component in componentList)
            {
                component.OnStart();
            }
        }
        
        public override void OnUpdate(LFloat deltaTime)
        {
            foreach (var component in componentList)
            {
                component.OnUpdate(deltaTime);
            }
        }
        
        public override void OnDestroy()
        {
            foreach (var component in componentList)
            {
                component.OnDestroy();
            }
        }

        public virtual void OnLPTriggerEnter(ColliderProxy other)
        {
            
        }

        public virtual  void OnLPTriggerStay(ColliderProxy other)
        {
            
        }

        public virtual  void OnLPTriggerExit(ColliderProxy other)
        {
            
        }
    }
}