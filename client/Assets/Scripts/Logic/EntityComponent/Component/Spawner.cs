using System;

namespace LockStepEngine
{
    [Serializable]
    public class Spawner : Entity
    {
        public SpawnerInfo Info = new SpawnerInfo();
        public LFloat Timer;

        public override void OnStart()
        {
            Timer = Info.spawnTime;
        }

        public override void OnUpdate(LFloat deltaTime)
        {
            Timer += deltaTime;
            if (Timer > Info.spawnTime)
            {
                Timer = LFloat.zero;
                
            }
        }

        public void Spawn()
        {
            if (GameStateService.CurEnemyCount >= GameStateService.MaxEnemyCount)
            {
                return;
            }

            GameStateService.CurEnemyCount++;
            GameStateService.CreateEntity<Enemy>(Info.prefabId, Info.spawnPoint);
        }
    }
}