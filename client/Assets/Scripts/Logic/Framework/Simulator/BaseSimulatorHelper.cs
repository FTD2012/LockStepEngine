namespace LockStepEngine
{
    public class BaseSimulatorHelper
    {
        public int Tick => world.Tick;
        protected World world;
        protected IServiceContainer serviceContainer;

        public BaseSimulatorHelper(IServiceContainer _serviceContainer, World _world)
        {
            world = _world;
            serviceContainer = _serviceContainer;
        }
    }
}