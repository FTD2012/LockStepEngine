using System;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms;

namespace LockStepEngine
{
    public class SimulatorService : BaseGameService, ISimulatorService, IDebugService
    {
        public static SimulatorService Instance { get; private set; }
        public int debugRockBackToTick;

        public const long MinMissFrameReqTickDiff = 10;
        public const long MaxSimulationMsPerFrame = 20;
        public const int MaxPredictFrameCount = 30;

        public World World => world;
        public int Ping => cmdBuffer?.Ping ?? 0;
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
        private byte[] allActors;
        private int tickSinceGameStart;
        private bool isInitVideo;
        private int tickOnLastJumpTo;
        private long timestampOnLastJumpToMS;
        private bool isDebugRollback;
        private IManagerContainer managerContainer;
        private IServiceContainer serviceContainer;
        private bool hasReceiveInputMsg;
        private Msg_G2C_GameStartInfo gameStartInfo;
        private Msg_ReqMissFrame videoFrame;
        private PlayerInput[] playerInputs => world.PlayerInputs;

        public SimulatorService()
        {
            Instance = this;
        }

        public override void InitReference(IServiceContainer _serviceContainer, IManagerContainer _managerContainer)
        {
            base.InitReference(_serviceContainer, _managerContainer);
            serviceContainer = _serviceContainer;
            managerContainer = _managerContainer;
        }

        public override void OnStart()
        {
            SnapshotFrameInterval = 1;
            if (constantStateService.IsVideoMode)
            {
                SnapshotFrameInterval = constantStateService.SnapshotFrameInterval;
            }

            cmdBuffer = new FrameBuffer(this, networkService, 2000, SnapshotFrameInterval, MaxPredictFrameCount);
            world = new World();
            hashHelper = new HashHelper(serviceContainer, world, networkService, cmdBuffer);
            dumpHelper = new DumpHelper(serviceContainer, world, hashHelper);
        }

        public void OnGameCreate(int targetFps, byte localActorId, byte actorAccount, bool isNeedRender = true)
        {
            FrameBuffer.debugMainActorId = localActorId;
            allActors = new byte[actorAccount];
            for (byte i = 0; i < actorAccount; i++)
            {
                allActors[i] = i;
            }

            GLog.Info("GameCreate " + LocalActorId);
            constantStateService.LocalActorId = LocalActorId;
            world.StartSimulate(serviceContainer, managerContainer);
            EventHelper.Trigger(EventType.LevelLoadProgress, 1f);
        }

        public void StartSimulate()
        {
            if (IsRunning)
            {
                GLog.Error("Game already started.");
                return;
            }

            IsRunning = true;
            if (constantStateService.IsClientMode)
            {
                GameStartTimestampMS = LTime.RealTimeSinceStartUpMS;
            }

            world.StartGame(gameStartInfo, LocalActorId);
            GLog.Info("GameStart " + LocalActorId);
            EventHelper.Trigger(EventType.LevelLoadProgress, null);

            while (InputTick < PreSendInputCount)
            {
                SendInput(InputTick++);
            }
        }

        public void Trace(string msg, bool isNewLine = false, bool isNeedLogTrace = false)
        {
            dumpHelper.Trace(msg, isNewLine, isNeedLogTrace);
        }

        public void JumpTo(int tick)
        {
            if (tick == world.Tick || tick + 1 == world.Tick)
            {
                return;
            }

            tick = LMath.Min(tick, videoFrame.frames.Length - 1);
            var time = LTime.RealTimeSinceStartUpMS + 0.05f;
            if (!isInitVideo)
            {
                constantStateService.IsVideoLoading = true;
                while (world.Tick < videoFrame.frames.Length)
                {
                    var sFrame = videoFrame.frames[world.Tick];
                    Simulate(sFrame, true);
                    if (LTime.RealTimeSinceStartUpMS > time)
                    {
                        EventHelper.Trigger(EventType.VideoLoadProgress, world.Tick * 1.0f / videoFrame.frames.Length);
                        return;
                    }
                }
            }

            if (world.Tick > tick)
            {
                RollbackTo(tick, videoFrame.frames.Length, false);
            }

            while (world.Tick <= tick)
            {
                var sFrame = videoFrame.frames[world.Tick];
                Simulate(sFrame, false);
            }
            
            viewService.RebindAllEntityes();
            timestampOnLastJumpToMS = LTime.RealTimeSinceStartUpMS;
            tickOnLastJumpTo = tick;
        }

        public void RunVideo()
        {
            if (tickOnLastJumpTo == world.Tick)
            {
                timestampOnLastJumpToMS = LTime.RealTimeSinceStartUpMS;
                tickOnLastJumpTo = world.Tick;
            }

            var frameDeltatime = (LTime.TimeSinceLevelLoad - timestampOnLastJumpToMS) * 1000;
            var targetTick = Math.Ceiling(frameDeltatime / NetworkDefine.UPDATE_DELTATIME) + tickOnLastJumpTo;
            while (world.Tick <= targetTick)
            {
                if (world.Tick < videoFrame.frames.Length)
                {
                    var sFrame = videoFrame.frames[world.Tick];
                    Simulate(sFrame, false);
                }
                else
                {
                    break;
                }
            }
        }

        public void OnUpdata(float deltaTime)
        {
            if (!IsRunning)
            {
                return;;
            }

            if (hasReceiveInputMsg)
            {
                if (GameStartTimestampMS == -1)
                {
                    GameStartTimestampMS = LTime.RealTimeSinceStartUpMS;
                }
            }

            if (GameStartTimestampMS <= 0)
            {
                return;
            }
            
            tickSinceGameStart = (int) (LTime.RealTimeSinceStartUpMS - GameStartTimestampMS) / NetworkDefine.UPDATE_DELTATIME);
            if (constantStateService.IsVideoMode)
            {
                return;
            }

            if (debugRockBackToTick > 0)
            {
                GetService<ICommonStateService>().IsPause = true;
                RollbackTo(debugRockBackToTick, 0, false);
                debugRockBackToTick = -1;
            }

            if (commonStateService.IsPause)
            {
                return;
            }

            cmdBuffer.OnUpdate(deltaTime);

            if (constantStateService.IsClientMode)
            {
                OnClientUpdate();
            }
            else
            {
                while (InputTick <= InputTargetTick)
                {
                    SendInput(InputTick++);
                }
                
                
            }
        }

        private void OnClientUpdate()
        {
            int maxRollbackCount = 5;
            if (isDebugRollback && world.Tick > maxRollbackCount && world.Tick % maxRollbackCount == 0)
            {
                var rawTick = world.Tick;
                var revertCount = LRandom.Range(1, maxRollbackCount);
                for (int i = 0; i < revertCount; i++)
                {
                    var input = new Msg_PlayerInput(world.Tick, LocalActorId, inputService.GetDebugInputCmdList());
                    var frame = new ServerFrame()
                    {
                        tick = rawTick - i,
                        inputs = new Msg_PlayerInput[] { input }
                    };
                    cmdBuffer.ForcePushDebugFrame(frame);
                }
                
                GLog.Info("RollbackTo " + (world.Tick - revertCount));
                if (!RollbackTo(world.Tick - revertCount, world.Tick))
                {
                    commonStateService.IsPause = true;
                    return;
                }

                while (world.Tick < rawTick)
                {
                    var sFrame = cmdBuffer.GetServerFrame(world.Tick);
                    cmdBuffer.PushLocalFrame(sFrame);
                    Simulate(sFrame);
                    if (commonStateService.IsPause)
                    {
                        return;
                    }
                }
            }

            while (world.Tick < TargetTick)
            {
                FramePredictCount = 0;
                var input = new Msg_PlayerInput(world.Tick, LocalActorId, inputService.GetInputCmdList());
                var frame = new ServerFrame()
                {
                    tick = world.Tick,
                    inputs = new Msg_PlayerInput[] { input }
                };
                cmdBuffer.PushLocalFrame(frame);
                cmdBuffer.PushServerFrames(new ServerFrame[] { frame });
                Simulate(cmdBuffer.GetFrame(world.Tick));
                if (commonStateService.IsPause)
                {
                    return;
                }
            }
        }

        private void DoNormalUpdate()
        {
            var maxContinueServerTick = cmdBuffer.MaxContinueServerTick;
            if ((world.Tick - maxContinueServerTick) > MaxPredictFrameCount)
            {
                return;
            }

            var minTickToBackup = maxContinueServerTick - (maxContinueServerTick % SnapshotFrameInterval);
            var deadline = LTime.RealTimeSinceStartUpMS + MaxSimulationMsPerFrame;
            while (world.Tick < cmdBuffer.CurTickInServer)
            {
                var tick = world.Tick;
                var sFrame = cmdBuffer.GetServerFrame(tick);
                if (sFrame == null)
                {
                    OnPursuingFrame();
                    return;
                }

                cmdBuffer.PushLocalFrame(sFrame);
                Simulate(sFrame, tick == minTickToBackup);
                if (LTime.RealTimeSinceStartUpMS > deadline)
                {
                    OnPursuingFrame();
                    return;
                }
            }
            
            if (constantStateService.IsPursueFrame)
            {
                constantStateService.IsPursueFrame = false;
                EventHelper.Trigger(EventType.PursueFrameDone);
            }


            // Roll back
            if (cmdBuffer.IsNeedRollback)
            {
                RollbackTo(cmdBuffer.NextTickToCheck, maxContinueServerTick);
                CleanUselessSnapshot(System.Math.Min(cmdBuffer.NextTickToCheck - 1, world.Tick));

                minTickToBackup = System.Math.Max(minTickToBackup, world.Tick + 1);
                while (world.Tick <= maxContinueServerTick)
                {
                    var sFrame = cmdBuffer.GetServerFrame(world.Tick);
                    cmdBuffer.PushLocalFrame(sFrame);
                    Simulate(sFrame, world.Tick == minTickToBackup);
                }
            }
        }

        private void SendInput(int curTick)
        {
            var input = new Msg_PlayerInput(curTick, LocalActorId, inputService.GetInputCmdList());
            var cFrame = new ServerFrame();
            var inputs = new Msg_PlayerInput[allActors.Length];
            inputs[LocalActorId] = input;
            cFrame.Inputs = inputs;
            FillInputWithLastFrame(cFrame);
            cmdBuffer.PushLocalFrame(cFrame);
            if (curTick > cmdBuffer.MaxServerTickInBuffer)
            {
                cmdBuffer.SendInput(input);
            }
        }

        private void Simulate(ServerFrame frame, bool isNeedGenShap = true)
        {
            Step(frame, isNeedGenShap);
        }

        private void Predict(ServerFrame frame, bool isNeedGenSnap = true)
        {
            Step(frame, isNeedGenSnap);
        }

        private bool RollbackTo(int tick, int maxContinueServerTick, bool isNeedClear = true)
        {
            world.RollbackTo(tick, maxContinueServerTick, isNeedClear);
            var hash = commonStateService.Hash;
            var curHash = hashHelper.CalcHash();
            if (hash != curHash)
            {
                GLog.Error($"tick:{tick} Rollback error: Hash isDiff oldHash ={hash}  curHash{curHash}");
                dumpHelper.DumpToFile(true);
                return false;
            }

            return true;
        }

        private void Step(ServerFrame frame, bool isNeedGenSnap = true)
        {
            commonStateService.SetTick(world.Tick);
            var hash = hashHelper.CalcHash();
            commonStateService.Hash = hash;
            timeMachineService.Backup(world.Tick);
            DumpFrame(hash);
            hash = hashHelper.CalcHash(true);
            hashHelper.SetHash(world.Tick, hash);
            ProcessInputQueue(frame);
            world.Step(isNeedGenSnap);
            dumpHelper.OnFrameEnd();
            var tick = world.Tick;
            cmdBuffer.SetClientTick(tick);
            if (isNeedGenSnap && tick % SnapshotFrameInterval == 0)
            {
                CleanUselessSnapshot(System.Math.Min(cmdBuffer.NextTickToCheck - 1, world.Tick));
            }
        }
        
        private void CleanUselessSnapshot(int tick)
        {
            //TODO
        }

        private void DumpFrame(int hash)
        {
            if (constantStateService.IsClientMode)
            {
                dumpHelper.DumpFrame(!hashHelper.TryGetValue(world.Tick, out var val));
            }
            else
            {
                dumpHelper.DumpFrame(true);
            }
        }

        private void FillInputWithLastFrame(ServerFrame frame)
        {
            int tick = frame.tick;
            var inputs = frame.Inputs;
            var lastServerInputs = tick == 0 ? null : cmdBuffer.GetFrame(tick - 1).Inputs;
            var myInput = inputs[LocalActorId];
            for (int i = 0; i < allActors.Length; i++)
            {
                inputs[i] = new Msg_PlayerInput(tick, allActors[i], lastServerInputs?[i]?.InputCommands.ToList());
            }

            inputs[LocalActorId] = myInput;
        }

        private void ProcessInputQueue(ServerFrame frame)
        {
            var inputs = frame.Inputs;
            foreach (var playerInput in playerInputs)
            {
                playerInput.Reset();
            }

            foreach (var input in inputs)
            {
                if (input.InputCommands == null || input.ActorId >= playerInputs.Length)
                {
                    continue;;
                }

                var inputEntity = playerInputs[input.ActorId];
                foreach (var command in input.InputCommands)
                {
                    GLog.Info( input.ActorId + " >> " + input.Tick + ": " + input.InputCommands.Count());
                    inputService.Execute(command, inputEntity);
                }
            }
        }

        private void OnPursuingFrame()
        {
            constantStateService.IsPursueFrame = true;
            GLog.Info($"PurchaseServering curTick:" + world.Tick);
            var progress = world.Tick * 1.0f / cmdBuffer.CurTickInServer;
            EventHelper.Trigger(EventType.PursueFrameProcess, progress);
        }

        private void OnEvent_BorderVideoFrame(object param)
        {
            videoFrame = param as Msg_ReqMissFrame;
        }

        private void OnEvent_OnServerFrame(object param)
        {
            var msg = param as Msg_
        }
        
        
        public override void OnDestroy()
        {
            IsRunning = false;
            dumpHelper.DumpAll();
        }
        
    }
}