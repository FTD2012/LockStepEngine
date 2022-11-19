using System;

namespace LockStepEngine
{
    [Serializable]
    public class Player : Entity
    {
        public int localId;
        public PlayerInput input = new PlayerInput();
        public CMover mover = new CMover();
        
        
    }
}