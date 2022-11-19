namespace LockStepEngine
{
    public class ServiceReferenceHolder
    {
        protected IServiceContainer serviceContainer;
        protected IECSFacadeService ecsFacadeService;

        protected IRandomService randomService;
        protected ITimeMachineService timeMachineService;
        protected IConstantStateService constantStateService;
        protected IViewService viewService;
        protected IAudioService audioService;
        protected IInputService inputService;
        protected IMap2DService map2DService;
        protected IResService resService;
        protected IEffectServicr effectServicr;
        protected IEventRegisterService eventRegisterService;
        protected IIdService idService;
        protected ICommonStateService commonStateService;
        protected IDebugService debugService;
        
        protected T GetService<T>() where T : IService
        {
            return serviceContainer.Get<T>();
        }

        public virtual void InitReference(IServiceContainer container, IManagerContainer managerContainer)
        {
            serviceContainer = container;
            ecsFacadeService = container.Get<IECSFacadeService>();
            randomService = container.Get<IRandomService>();
            timeMachineService = container.Get<ITimeMachineService>();
            constantStateService = container.Get<IConstantStateService>();
            viewService = container.Get<IViewService>();
            audioService = container.Get<IAudioService>();
            inputService = container.Get<IInputService>();
            map2DService = container.Get<IMap2DService>();
            resService = container.Get<IResService>();
            effectServicr = container.Get<IEffectServicr>();
            eventRegisterService = container.Get<IEventRegisterService>();
            idService = container.Get<IIdService>();
            commonStateService = container.Get<ICommonStateService>();
            debugService = container.Get<IDebugService>();
        }
    }
}