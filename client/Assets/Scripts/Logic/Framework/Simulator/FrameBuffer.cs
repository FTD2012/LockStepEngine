using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LockStepEngine
{
    public interface IFrameBuffer
    {
        void ForcePushDebugFrame(ServerFrame frame);
        void PushLocalFrame(ServerFrame frame);
        void PushServerFrames(ServerFrame[] frames, bool isNeedDebugCheck = true);
        void PushMissServerFrames(ServerFrame[] frames, bool isNeedDebugCheck = true);
        void OnPlayerPing(Msg_G2C_PlayerPing input);
        ServerFrame GetFrame(int tick);
        ServerFrame GetServerFrame(int tick);
        ServerFrame GetLocalFrame(int tick);
        void SetClientTick(int tick);
        void SendInput(Msg_PlayerInput input);
        void OnUpdate(float deltaTime);
        int NextTickToCheck { get; }
        int MaxServerTickInBuffer { get; }
        bool IsNeedRollback { get; }
        int MaxContinueServerTick { get; }
        int CurTickInServer { get; }
        int Ping { get; }
        int Delay { get; }
    }

    public class FrameBuffer : IFrameBuffer
    {
        public class PredictCountHelper
        {
            public int missTick = -1;
            public int nextCheckMissTick = 0;
            public bool hasMissTick;

            private SimulatorService simulatorService;
            private FrameBuffer cmdBuffer;
            private float timer;
            private float checkInterval = 0.5f;
            private float incPercent = 0.3f;
            private float targetPreSendTick;
            private float oldPercent = 0.6f;

            public PredictCountHelper(SimulatorService _simulatorService, FrameBuffer _cmdBuffer)
            {
                simulatorService = _simulatorService;
                cmdBuffer = _cmdBuffer;
            }

            public void OnUpdate(float deltaTime)
            {
                timer += deltaTime;
                if (timer > checkInterval)
                {
                    timer = 0;
                    if (!hasMissTick)
                    {
                        var preSend = cmdBuffer.maxPing * 1.0f / NetworkDefine.UPDATE_DELTATIME;
                        targetPreSendTick = targetPreSendTick * oldPercent + preSend * (1 - oldPercent);
                        simulatorService.PreSendInputCount = LMath.Clamp((int)Math.Ceiling(targetPreSendTick), 1, 60);
                        GLog.Warning($"Shrink preSend buffer old:{simulatorService.PreSendInputCount} new:{targetPreSendTick} " + $"PING: min:{cmdBuffer.minPing} max:{cmdBuffer.maxPing} avg:{cmdBuffer.Ping}");
                    }
                    hasMissTick = false;
                }

                if (missTick != -1)
                {
                    var delayTick = simulatorService.TargetTick - missTick;
                    var preSendTick = simulatorService.PreSendInputCount + (int)Math.Ceiling(delayTick * incPercent);
                    simulatorService.PreSendInputCount = LMath.Clamp(preSendTick, 1, 60);
                    nextCheckMissTick = simulatorService.TargetTick;
                    missTick = -1;
                    hasMissTick = true;
                    GLog.Warning($"Expend preSend buffer old:{simulatorService.PreSendInputCount} new:{targetPreSendTick}");
                }
            }
        }

        public static byte debugMainActorId;
        public int Ping { get; private set; }
        public int Delay { get; private set; }
        public int CurTickInServer { get; private set; }
        public int NextTickToCheck { get; private set; }
        public int MaxServerTickInBuffer { get; private set; }
        public bool IsNeedRollback { get; private set; }
        public int MaxContinueServerTick { get; private set; }
        public byte LocalId;
        public INetworkService NetworkService;

        private int nextClientTick;
        private long guessServerStartTimestamp;
        private long historyMinPing;
        private long minPing;
        private long maxPing;
        private float pingTimer;
        private List<long> pings;
        private List<long> delays;
        private Dictionary<int, long> tick2SendTimestamp;

        private int maxClientPredictFrameCount;
        private int bufferSize;
        private int spaceRoolbackNeed;
        private int maxServerOverFrameCount;
        private ServerFrame[] serverBuffer;
        private ServerFrame[] clientBuffer;
        private PredictCountHelper predictCountHelper;
        private SimulatorService simulatorService;

        public FrameBuffer(SimulatorService _simulatorService, INetworkService _networkService, int _bufferSize, int _snapshotFrameInterval, int _maxClientPredictFrameCount)
        {
            MaxServerTickInBuffer = -1;
            guessServerStartTimestamp = Int64.MaxValue;
            historyMinPing = Int64.MaxValue;
            minPing = Int64.MaxValue;
            maxPing = Int64.MinValue;
            pings = new List<long>();
            delays = new List<long>();
            tick2SendTimestamp = new Dictionary<int, long>();
            predictCountHelper = new PredictCountHelper(_simulatorService, this);

            simulatorService = _simulatorService;
            bufferSize = _bufferSize;
            NetworkService = _networkService;
            maxClientPredictFrameCount = _maxClientPredictFrameCount;
            spaceRoolbackNeed = _snapshotFrameInterval * 2;
            maxServerOverFrameCount = bufferSize - spaceRoolbackNeed;
            serverBuffer = new ServerFrame[bufferSize];
            clientBuffer = new ServerFrame[bufferSize];
        }

        public void SetClientTick(int tick)
        {
            nextClientTick = tick + 1;
        }

        public void PushLocalFrame(ServerFrame frame)
        {
            var index = frame.tick % bufferSize;
            clientBuffer[index] = frame;
        }

        public void OnPlayerPing(Msg_G2C_PlayerPing msg)
        {
            var ping = LTime.RealTimeSinceStartUpMS - msg.sendTimestamp;
            pings.Add(ping);
            if (ping > maxPing)
            {
                maxPing = ping;
            }

            if (ping < minPing)
            {
                minPing = ping;
                guessServerStartTimestamp = (LTime.RealTimeSinceStartUpMS - msg.timeSinceServerStart) - minPing / 2;
            }
        }

        public void PushMissServerFrames(ServerFrame[] frames, bool isNeedDebugCheck = true)
        {
            PushServerFrames(frames, isNeedDebugCheck);
            NetworkService.SendMissFrameReqAck(MaxContinueServerTick + 1);
        }

        public void ForcePushDebugFrame(ServerFrame data)
        {
            var targetIdx = data.tick % bufferSize;
            serverBuffer[targetIdx] = data;
            clientBuffer[targetIdx] = data;
        }

        public void PushServerFrames(ServerFrame[] frames, bool isNeedDebugCheck = true)
        {
            var count = frames.Length;
            for (int i = 0; i < count; i++)
            {
                var data = frames[i];
                //Debug.Log("PushServerFrames" + data.tick);
                if (tick2SendTimestamp.TryGetValue(data.tick, out var sendTick))
                {
                    var delay = LTime.RealTimeSinceStartUpMS - sendTick;
                    delays.Add(delay);
                    tick2SendTimestamp.Remove(data.tick);
                }

                if (data.tick < NextTickToCheck)
                {
                    //the frame is already checked
                    return;
                }

                if (data.tick > CurTickInServer)
                {
                    CurTickInServer = data.tick;
                }

                if (data.tick >= NextTickToCheck + maxServerOverFrameCount - 1)
                {
                    return;
                }

                if (data.tick > MaxServerTickInBuffer)
                {
                    MaxServerTickInBuffer = data.tick;
                }

                var targetIdx = data.tick % bufferSize;
                if (serverBuffer[targetIdx] == null || serverBuffer[targetIdx].tick != data.tick)
                {
                    serverBuffer[targetIdx] = data;
                    if (data.tick > predictCountHelper.nextCheckMissTick && data.Inputs[LocalId].IsMiss && predictCountHelper.missTick == -1)
                    {
                        predictCountHelper.missTick = data.tick;
                    }
                }
            }
        }


        public void OnUpdate(float deltaTime)
        {
            NetworkService.SendPing(simulatorService.LocalActorId, LTime.RealTimeSinceStartUpMS);
            predictCountHelper.OnUpdate(deltaTime);
            int worldTick = simulatorService.World.Tick;
            UpdatePingVal(deltaTime);

            //Confirm frames
            IsNeedRollback = false;
            while (NextTickToCheck <= MaxServerTickInBuffer && NextTickToCheck < worldTick)
            {
                var sIdx = NextTickToCheck % bufferSize;
                var cFrame = clientBuffer[sIdx];
                var sFrame = serverBuffer[sIdx];
                if (cFrame == null || cFrame.tick != NextTickToCheck || sFrame == null ||
                    sFrame.tick != NextTickToCheck)
                    break;
                //Check client guess input match the real input
                if (object.ReferenceEquals(sFrame, cFrame) || sFrame.Equals(cFrame))
                {
                    NextTickToCheck++;
                }
                else
                {
                    IsNeedRollback = true;
                    break;
                }
            }

            //Request miss frame data
            int tick = NextTickToCheck;
            for (; tick <= MaxServerTickInBuffer; tick++)
            {
                var idx = tick % bufferSize;
                if (serverBuffer[idx] == null || serverBuffer[idx].tick != tick)
                {
                    break;
                }
            }

            MaxContinueServerTick = tick - 1;
            if (MaxContinueServerTick <= 0) return;
            if (MaxContinueServerTick < CurTickInServer // has some middle frame pack was lost
                || nextClientTick >
                MaxContinueServerTick + (maxClientPredictFrameCount - 3) //client has predict too much
               )
            {
                GLog.Info("SendMissFrameReq " + MaxContinueServerTick);
                NetworkService.SendMissFrameReq(MaxContinueServerTick);
            }
        }

        private void UpdatePingVal(float deltaTime)
        {
            pingTimer += deltaTime;
            if (pingTimer > 0.5f)
            {
                pingTimer = 0;
                Delay = (int) (delays.Sum() / LMath.Max(delays.Count, 1));
                delays.Clear();
                Ping = (int) (pings.Sum() / LMath.Max(pings.Count, 1));
                pings.Clear();

                if (minPing < historyMinPing && simulatorService.GameStartTimestampMS != -1)
                {
                    historyMinPing = minPing;
                    GLog.Warning($"Recalc _gameStartTimestampMs {simulatorService.GameStartTimestampMS} _guessServerStartTimestamp:{guessServerStartTimestamp}");
                    simulatorService.GameStartTimestampMS = LMath.Min(guessServerStartTimestamp, simulatorService.GameStartTimestampMS);
                }

                minPing = Int64.MaxValue;
                maxPing = Int64.MinValue;
            }
        }

        public void SendInput(Msg_PlayerInput input)
        {
            tick2SendTimestamp[input.Tick] = LTime.RealTimeSinceStartUpMS;
#if DEBUG_SHOW_INPUT
            var cmd = input.Commands[0];
            var playerInput = new Deserializer(cmd.content).Parse<Lockstep.Game. PlayerInput>();
            if (playerInput.inputUV != LVector2.zero) {
                Debug.Log($"SendInput tick:{input.Tick} uv:{playerInput.inputUV}");
            }
#endif
            NetworkService.SendInput(input);
        }

        public ServerFrame GetFrame(int tick)
        {
            var sFrame = GetServerFrame(tick);
            if (sFrame != null)
            {
                return sFrame;
            }

            return GetLocalFrame(tick);
        }

        public ServerFrame GetServerFrame(int tick)
        {
            if (tick > MaxServerTickInBuffer)
            {
                return null;
            }

            return _GetFrame(serverBuffer, tick);
        }

        public ServerFrame GetLocalFrame(int tick)
        {
            if (tick >= nextClientTick)
            {
                return null;
            }

            return _GetFrame(clientBuffer, tick);
        }

        private ServerFrame _GetFrame(ServerFrame[] buffer, int tick)
        {
            var idx = tick % bufferSize;
            var frame = buffer[idx];
            if (frame == null) return null;
            if (frame.tick != tick) return null;
            return frame;
        }
    }
}