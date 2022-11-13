namespace LockStepEngine
{
    public class ServiceReferenceHolder
    {
        protected IServiceContainer serviceContainer;
        protected ICommonStateService commonStateService;
        
        protected T GetService<T>() where T : IService
        {
            return serviceContainer.Get<T>();
        }

        public virtual void InitReference(IServiceContainer container, IManagerContainer managerContainer)
        {
            serviceContainer = container;
            commonStateService = container.Get<ICommonStateService>();
        }
    }
}