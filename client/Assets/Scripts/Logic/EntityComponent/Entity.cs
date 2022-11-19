using System;
using LockStepEngine.Collision2D;

namespace LockStepEngine
{
    [Serializable]
    [NoBackup]
    public class Entity : BaseEntity
    {
        public CRigidbody rigidbody = new CRigidbody();
        public ColliderData colliderData = new ColliderData();
        public CAnimator animator = new CAnimator();
        public CSkillBox

        public LFloat moveSpd = 5;
    }
}