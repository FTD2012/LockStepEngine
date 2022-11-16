namespace LockStepEngine
{
    public class SimulatorService //: BaseGameService, ISimulatorService, IDebugService
    {
        public static SimulatorService Instance { get; private set; }
        public int debugRockBackToTick;

        public const long MinMissFrameReqTickDiff = 10;
        public const long MaxSimulationMsPerFrame = 20;
        public const int MaxPredictFrameCount = 30;

        public World World => world;
        public int Pint => cmdBuffer?.Ping ?? 0;
        public int Delay => cmdBuffer?.Delay ?? 0;
        public int TargetTick => tickSinceGameStart + FramePredictCount;
        public int InputTargetTick => tickSinceGameStart + PreSendInputCount;
        public int InputTick { get; set; }
        public byte LocalActorId { get; private set; }
        public bool IsRunning { get; set; }
        public int FramePredictCount { get; set; } // TODO: ljm >>> should change accounding current network's delay
        public int PreSendInputCount { get; set; } // TODO: ljm >>> should change accounding current network's delay
        public long GameStartTimestampMS { get; set; }
        public int SnapshotFrameInterval { get; set; }
        
        

        private World world;
        private IFrameBuffer cmdBuffer;
        private HashHelper hashHelper;
        private DumpHelper dumpHelper;
        // private Msg_G2C_GameStartInfo gameStartInfo;
        private byte[] allActors;
        private int tickSinceGameStart;
        // private Msg_ReqMissFrame videoFrame;
        private bool isInitVideo;
        private int tickOnLastJumpTo;
        private long timestampOnLastJumpToMS;
        private bool isDebugRollback;
        private IManagerContainer managerContainer;
        private IServiceContainer serviceContainer;
        private bool hasReceiveInputMsg;
        // private PlayerInput[] playerInputs => world.PlayerInputs;

        public SimulatorService()
        {
            Instance = this;
        }

        public /*override*/ void InitReference(IServiceContainer _serviceContainer, IManagerContainer _managerContainer)
        {
            // base.InitReference(_serviceContainer, _managerContainer);
            serviceContainer = _serviceContainer;
            managerContainer = _managerContainer;
        }

        public /*override*/ void OnStart()
        {
            SnapshotFrameInterval = 1;
            // if (constantStateService.IsVideoMode)
            // {
            //     SnapshotFrameInterval = constantStateService.SnapshotFrameInterval;
            // }
            
            cmdBuffer = new FrameBuffer()
            
        }
        
    }
}