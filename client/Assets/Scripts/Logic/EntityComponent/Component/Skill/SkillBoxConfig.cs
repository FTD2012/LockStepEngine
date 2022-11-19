using System.Collections.Generic;
using UnityEngine;

namespace LockStepEngine
{
    [CreateAssetMenu(menuName = "SkillInfo")]
    public class SkillBoxConfig : ScriptableObject
    {
        public List<SkillInfo> skillInfos = new List<SkillInfo>();
        private bool hasInit;

        public void CheckInit()
        {
            if (hasInit)
            {
                return;
            }

            hasInit = true;
            foreach (var info in skillInfos)
            {
                info.Init();
            }
        }
    }
}