namespace LockStepEngine
{
    public interface ICommonStateService : IService
    {
        int Tick { get; }
        LFloat DeltaTime { get; }
        LFloat TimeSinceGameStart { get; }
        int Hash { get; set; }
        bool IsPause { get; set; }
        void SetTick(int val);
        void SetDeltaTime(LFloat val);
        void SetTimeSinceGameStart(LFloat val);
    }
}