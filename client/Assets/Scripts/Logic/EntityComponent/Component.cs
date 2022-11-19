using System;
using LockStepEngine.Interface;

namespace LockStepEngine
{
    [Serializable]
    [NoBackup]
    public partial class Component : BaseComponent
    {
        public Entity entity => (Entity) baseEntity;
        public IGameStateService GameStateService => entity.GameStateService;
        public IDebugService DebugService => entity.DebugService;
    }
}