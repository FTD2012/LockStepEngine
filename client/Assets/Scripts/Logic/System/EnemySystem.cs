namespace LockStepEngine
{
    public class EnemySystem : BaseSystem
    {
        private Spawner[] Spawners => gameStateService.getSpawners();
        private Enemy[] AllEnemy => gameStateService.GetEnemies();
        public override void OnStart()
        {
            for (int i = 0; i < 3; i++)
            {
                var configId = 100 + i;
                var config = gameConfigService.GetEntityConfig(configId) as SpawnerConfig;
                gameStateService.CreateEntity<Spawner>(configId, config.entity.Info.spawnPoint);
            }

            foreach (var spawner in Spawners)
            {
                spawner.ServiceContainer = serviceContainer;
                spawner.GameStateService = gameStateService;
                spawner.DebugService = debugService;
                spawner.OnStart();
            }
        }

        public override void OnUpdate(LFloat deltaTime)
        {
            foreach (var spawner in Spawners)
            {
                spawner.OnUpdate(deltaTime);
            }

            foreach (var enemy in AllEnemy)
            {
                enemy.OnUpdate(deltaTime);
            }
        }
    }
}