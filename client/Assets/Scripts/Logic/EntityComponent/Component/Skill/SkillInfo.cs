using System;
using System.Collections.Generic;

namespace LockStepEngine
{
    [Serializable]
    public class SkillColliderInfo
    {
        public LVector2 pos;
        public LVector2 size;
        public LFloat radius;
        public LFloat deg = new LFloat(180);
        public LFloat maxY;

        public bool IsCircle => radius > 0;
    }

    [Serializable]
    public class SkillPart
    {
        public bool DebugShow;
        public LFloat moveSpd;
        public LFloat startFrame;
        public LFloat startTimer => startFrame * AnimFrameScale;
        public SkillColliderInfo colliderInfo;
        public LVector3 impulseForce;
        public bool needForce;
        public bool isNeedForce;

        public LFloat interval;
        public int otherCount;
        public int damage;
        public static LFloat AnimFrameScale = new LFloat(true, 1667);
        public LFloat DeadTimer => startTimer + interval * (otherCount + LFloat.half);

        public LFloat NextTriggerTimer(int counter)
        {
            return startTimer + interval * counter;
        }
    }
    
    [Serializable]
    public class SkillInfo
    {
        public string animName;
        public LFloat CD;
        public LFloat doneDelay;
        public int targetLayer;
        public LFloat maxPartTime;
        public List<SkillPart> parts = new List<SkillPart>();

        public void Init()
        {
            parts.Sort((a, b) => LMath.Sign(a.startFrame - b.startFrame));
            var time = LFloat.MinValue;
            foreach (var part in parts)
            {
                var partDeatTime = part.DeadTimer;
                if (partDeatTime > time)
                {
                    time = partDeatTime;
                }
            }

            maxPartTime = time + doneDelay;
        }
    }
}