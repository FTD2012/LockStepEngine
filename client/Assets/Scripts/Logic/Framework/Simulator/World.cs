using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LockStepEngine
{
    public class World : BaseGameService
    {
        public static Player MyPlayer;
        public static World Instance { get; set; }
        public static object MyPlayerTrans => MyPlayer?.engineTransform;
        public int Tick { get; set; }
        public PlayerInput[] PlayerInputs => gameStateService.GetPlayers().Select(a => a.input).ToArray();
        public List<BaseSystem> systems = new List<BaseSystem>();
        private bool hasStart;

        public void RollbackTo(int tick, int maxContinueServerTick, bool isNeedClear = true)
        {
            if (tick < 0)
            {
                GLog.Error("Target Tick invalid!" + tick);
                return;
            }
            GLog.Info($" Rollback diff:{Tick - tick} From{Tick}->{tick}  maxContinueServerTick:{maxContinueServerTick} {isNeedClear}");
            timeMachineService.RollbackTo(tick);
            commonStateService.SetTick(tick);
            Tick = tick;
        }

        public void StartSimulate(ServiceContainer _serviceContainer, IManagerContainer _managerContainer)
        {
            Instance = this;
            serviceContainer = _serviceContainer;
            RegisterSystems();
            if (!_serviceContainer.Get<IConstantStateService>().IsVideoMode)
            {
                RegisterSystem(new TraceLogSystem());
            }
            
            InitReference(_serviceContainer, _managerContainer);
            foreach (var manager in systems)
            {
                manager.InitReference(_serviceContainer, _managerContainer);
            }

            foreach (var manager in systems)
            {
                manager.OnAwake(_serviceContainer);
            }

            OnAwake(_serviceContainer);
            foreach (var manager in systems)
            {
                manager.OnStart();
            }

            OnStart();
        }

        public void StartGame(Msg_G2C_GameStartInfo gameStartInfo, int localPlayerId)
        {
            if (hasStart)
            {
                return;
            }
            hasStart = true;

            var playerInfos = gameStartInfo.UserInfos;
            var playerCount = playerInfos.Length;
            for (int i = 0; i < playerCount; i++)
            {
                var PrefabId = 0;
                var initPos = LVector2.zero;
                var player = gameStateService.CreateEntity<Player>(PrefabId, initPos);
                player.localId = i;
            }

            var allPlayer = gameStateService.GetPlayers();

            MyPlayer = allPlayer[localPlayerId];
        }

        public void Step()
        {
            if (commonStateService.IsPause)
            {
                return;
            }

            var deltaTime = new LFloat(true, 30);
            foreach (var system in systems)
            {
                if (system.enable)
                {
                    system.OnUpdate(deltaTime);
                }
            }

            Tick++;
        }

        public void RegisterSystems()
        {
            RegisterSystem(new HeroSystem());
            RegisterSystem(new EnemySystem());
            RegisterSystem(new PhysicSystem());
        }

        public void RegisterSystem(BaseSystem manager)
        {
            systems.Add(manager);
        }

        public override void OnApplicationQuit()
        {
            OnDestroy();
        }

        public override void OnDestroy()
        {
            foreach (var  manager in systems)
            {
                manager.OnDestroy();
            }
        }
    }
    
}