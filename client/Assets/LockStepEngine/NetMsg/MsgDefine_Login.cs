using System.Collections.Generic;

namespace LockStepEngine
{
    public class GameProperty : BaseMsg
    {
        public string Name;
        public short Type;
        public byte[] Data;
    }
    
    public class GameData : BaseMsg
    {
        public string UserName;
        public long UserId;
        public List<GameProperty> DataList;
    }
}