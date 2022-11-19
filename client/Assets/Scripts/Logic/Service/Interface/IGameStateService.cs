using UnityEngine;

namespace LockStepEngine
{
    public interface IGameStateService : IService
    {
        LFloat RemainTime { get; set; }
        LFloat DeltaTime { get; set; }
        int MaxEnemyCount { get; set; }
        int CurEnemyCount { get; set; }
        int CurEnemyId { get; set; }

        Enemy[] GetEnemies();
        Player[] GetPlayers();
        Spawner[] getSpawners();
        object GetEntity(int id);
        T CreateEntity<T>(int prefabId, LVector3 position);
        void DestroyEntity(BaseEntity entity);
    }
}