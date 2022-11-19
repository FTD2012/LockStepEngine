using System;

namespace LockStepEngine
{
    public interface IContext
    {
        
    }
    
    public interface INeedBackup
    {
        
    }

    public interface IEntity : INeedBackup
    {
        
    }

    public interface IComponent : INeedBackup
    {
        
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class NoBackupAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class BackupAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property)]
    public class ReRefBackupAttribute : Attribute
    {
        
    }
}