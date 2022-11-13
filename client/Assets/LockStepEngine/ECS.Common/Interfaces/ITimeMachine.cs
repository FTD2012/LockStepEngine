namespace LockStepEngine
{
    public interface ITimeMachine
    {
        int Tick { get; }
        void RollbackTo(int tick);
        void Backup(int tick);
        void Clean(int maxVerifiedTick);
    }
}