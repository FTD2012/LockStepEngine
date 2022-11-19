namespace LockStepEngine
{
    public interface IGameConfigService : IService
    {
        EntityConfig GetEntityConfig(int id);
        AnimatorConfig GetAnimatorConfig(int id);
        SkillBoxConfig GetSkillBoxConfig(int it);
        
        CollisionConfig CollisionConfig { get; }
        string RecorderFilePath { get; }
        string DumpStrPath { get; }
        Msg_G2C_GameStartInfo ClientModeInfo { get; }
    }
}