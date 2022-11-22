using System.Text;

namespace LockStepEngine
{
    public abstract class BaseService : ServiceReferenceHolder, IService, ILifeCycle, ITimeMachine, IHashCode, IDumpStr
    {
        public int Tick { get { return commonStateService.Tick; } }
        protected CommandBuffer cmdBuffer;

        protected BaseService()
        {
            cmdBuffer = new CommandBuffer();
            cmdBuffer.Init(this, GetRoobackFun());
        }

        public virtual void OnInit(object objParent) { }
        public virtual void OnAwake(IServiceContainer _serviceContainer) { }
        public virtual void OnStart() { }
        public virtual void OnDestroy() { }
        public virtual void OnApplicationQuit() { }
        public virtual void DumpStr(StringBuilder sb, string prefix) { }
        public virtual void Backup(int tick) { }
        public virtual void RollbackTo(int tick) { cmdBuffer?.Jump(Tick, tick); }
        public virtual void Clean(int maxVerfiedTick) { cmdBuffer?.Clean(maxVerfiedTick); }
        public virtual int GetHashCode(ref int idx) { return 0; }
        protected virtual FuncUndoCommand GetRoobackFun() { return null; }
    }
}