using System.Text;
using LockStepEngine.Serialization;

namespace LockStepEngine
{
    public interface IHashCode
    {
        int GetHashCode(ref int idx);
    }
    
    public interface IDumpStr
    {
        void DumpStr(StringBuilder sb, string prefix);
    }
    
    public interface IBackup : IDumpStr
    {
        void WriteBackup(Serializer writer);
        void ReadBackup(Deserializer reader);
    }

    public interface IAfterBackup
    {
        void OnAfterDeserialize();
    }
}