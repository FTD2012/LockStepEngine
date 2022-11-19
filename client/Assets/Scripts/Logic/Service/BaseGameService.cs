namespace LockStepEngine
{
    public abstract class BaseGameService : BaseService, IBaseGameManager
    {
        protected INetworkService networkService;
        protected ISimulatorService simulatorService;
        protected IUIService uiService;
        protected IGameStateService gameStateService;
        protected IGameEffectService gameEffectService;
        protected IGameAudioService gameAudioService;
        protected IGameConfigService gameConfigService;
        protected IGameViewService gameViewService;
        protected IGameResourceService gameResourceService;

        public override void InitReference(IServiceContainer serviceContainer, IManagerContainer managerContainer)
        {
            base.InitReference(serviceContainer, managerContainer);

            networkService = serviceContainer.Get<INetworkService>();
            simulatorService = serviceContainer.Get<ISimulatorService>();
            uiService = serviceContainer.Get<IUIService>();
            gameStateService = serviceContainer.Get<IGameStateService>();
            gameEffectService = serviceContainer.Get<IGameEffectService>();
            gameAudioService = serviceContainer.Get<IGameAudioService>();
            gameConfigService = serviceContainer.Get<IGameConfigService>();
            gameViewService = serviceContainer.Get<IGameViewService>();
            gameResourceService = serviceContainer.Get<IGameResourceService>();
        }
    }

    public class BaseSystem : BaseGameService
    {
        public bool enable = true;

        public virtual void OnUpdate(LFloat deltaTime)
        {
            
        }
    }
}