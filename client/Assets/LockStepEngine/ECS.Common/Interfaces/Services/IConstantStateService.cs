namespace LockStepEngine
{
    public interface IConstantStateService : IService
    {
        bool IsVideoLoading { get; set; }
        bool IsVideoMode { get; set; }
        bool IsRunVideo { get; set; }
        bool IsClientMode { get; set; }
        bool IsReconnecting { get; set; }
        bool IsPursueFrame { get; set; }
        string GameName { get; set; }
        int CurLevel { get; set; }
        IContext Context { get; set; }
        int SnapshotFrameInterval { get; set; }
        EPureModelType RunMode { get; set; }
        string ClinetConfigPath { get; }
        string RelPath { get; set; }
        byte LocalActorId { get; set; }
    }
}