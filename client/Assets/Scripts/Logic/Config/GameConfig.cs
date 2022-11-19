using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LockStepEngine
{
    [Serializable]
    public class EntityConfig
    {
        public virtual object Entity { get; }
        public string prefabPath;

        public void CopyTo(object dst)
        {
            if (Entity.GetType() != dst.GetType())
            {
                return;
            }

            FieldInfo[] fields = dst.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in fields)
            {
                var type = field.FieldType;
                if (typeof(INeedBackup).IsAssignableFrom(type))
                {
                    CopyTo(field.GetValue(dst), field.GetValue(Entity));
                }
                else
                {
                    field.SetValue(dst, field.GetValue(Entity));
                }
            }
        }

        void CopyTo(object dst, object src)
        {
            if (src.GetType() != dst.GetType())
            {
                return;
            }

            FieldInfo[] fields = dst.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in fields)
            {
                var type = field.FieldType;
                field.SetValue(dst, field.GetValue(src));
            }
        }
    }

    [Serializable]
    public class EnemyConfig : EntityConfig
    {
        public override object Entity => entity;
        public Entity entity => new Entity();
    }

    [Serializable]
    public class PlayerConfig : EntityConfig
    {
        public override object Entity => entity;
        public Player entity = new Player();
    }

    [Serializable]
    public class SpawnerConfig : EntityConfig
    {
        public override object Entity => entity;
        public Spawner entity = new Spawner();
    }

    [Serializable]
    public class CollisionConfig
    {
        public LVector3 pos;
        public LFloat worldSize = new LFloat(60);
        public LFloat minNodeSize = new LFloat(1);
        public LFloat loosenessval = new LFloat(true, 1250);
        public bool[] collisionMatrix = new bool[(int)ColliderLayerType.Count * (int)ColliderLayerType.Count];
        private string[] colliderLayerNames;

        public string[] ColliderLayerNames
        {
            get
            {
                if (colliderLayerNames == null || colliderLayerNames.Length <= 0)
                {
                    var list = new List<string>();
                    for (int i = 0; i < (int)ColliderLayerType.Count; i++)
                    {
                        list.Add(((ColliderLayerType)i).ToString());
                    }

                    colliderLayerNames = list.ToArray();
                }

                return colliderLayerNames;
            }
        }

        public void SetColliderPair(int a, int b, bool val)
        {
            collisionMatrix[a * (int)ColliderLayerType.Count + b] = val;
            collisionMatrix[b * (int)ColliderLayerType.Count + a] = val;
        }

        public bool GetColliderPair(int a, int b)
        {
            return collisionMatrix[a * (int)ColliderLayerType.Count];
        }
    }

    [CreateAssetMenu(menuName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public CollisionConfig CollisionConfig;
        public string RecorderFilePath;
        public string DumpStrPath;
        public Msg_G2C_GameStartInfo ClientModeInfo = new Msg_G2C_GameStartInfo();
        public List<PlayerConfig> PlayerConfigList = new List<PlayerConfig>();
        public List<EnemyConfig> EnemyConfigList = new List<EnemyConfig>();
        public List<SpawnerConfig> SpawnerConfigList = new List<SpawnerConfig>();
        public List<AnimatorConfig> AnimatorConfigList = new List<AnimatorConfig>();
        public List<SkillBoxConfig> SkillBoxConfigList = new List<SkillBoxConfig>();

        public void OnAwake()
        {
            foreach (var skillBoxConfig in SkillBoxConfigList)
            {
                skillBoxConfig.CheckInit();
            }            
        }

        private T GetConfig<T>(List<T> list, int id) where T : EntityConfig
        {
            if (id < 0 || id >= list.Count)
            {
                GLog.Error("Miss " + typeof(T) + " " + id);
                return null;
            }

            return list[id];
        }

        public EntityConfig GetEnemyConfig(int id)
        {
            return GetConfig(EnemyConfigList, id);
        }

        public EntityConfig GetPlayerConfig(int id)
        {
            return GetConfig(PlayerConfigList, id);
        }

        public EntityConfig GetSpawberConfig(int id)
        {
            return GetConfig(SpawnerConfigList, id);
        }

        public AnimatorConfig GetAnimatorConfig(int id)
        {
            return (id < 0 || id >= AnimatorConfigList.Count) ? null : AnimatorConfigList[id];
        }

        public SkillBoxConfig GetSkillBoxConfig(int id)
        {
            return (id < 0 || id >= SkillBoxConfigList.Count) ? null : SkillBoxConfigList[id];
        }
    }
}