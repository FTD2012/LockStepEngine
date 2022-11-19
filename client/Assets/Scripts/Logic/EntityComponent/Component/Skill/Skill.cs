using System;

namespace LockStepEngine
{
    public enum SkillStateType
    {
        Idle,
        Firing,
    }

    public interface ISkillEventHandler
    {
        void OnSkillStart(Skill skill);
        void OnSkillDone(Skill skill);
        void OnSkillPartStart(Skill skill);
    }
    
    [Serializable]
    public class Skill : INeedBackup
    {
        
    }
}