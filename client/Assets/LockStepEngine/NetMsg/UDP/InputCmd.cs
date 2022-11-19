using System;
using LockStepEngine.Serialization;
using LockStepEngine.Serialization;
using UnityEngine;

namespace LockStepEngine
{
    [Serializable]
    [SelfImplement]
    [Udp]
    public class InputCmd : BaseMsg
    {
        public byte[] content;

        public InputCmd()
        {
            
        }

        public InputCmd(byte type)
        {
            content = new[] { type };
        }

        public InputCmd(byte[] bytes)
        {
            content = bytes;
        }

        public bool Equals(InputCmd inputCmd)
        {
            if (inputCmd == null)
            {
                return false;
            }

            return content.EqualsEx(inputCmd.content);
        }

        public override bool Equals(object obj)
        {
            var inputCmd = obj as InputCmd;
            return Equals(inputCmd);
        }

        public override int GetHashCode()
        {
            return content.GetHashCode();
        }

        public override string ToString()
        {
            return $"t:{content[0]} content:{content?.Length ?? 0}";
        }

        public override void Serialize(Serializer write)
        {
            Debug.Assert(content != null && content.Length > 0 && content.Length < byte.MaxValue, $"!!!!!!!!! Input Cmd len{content?.Length ?? 0} should less then {byte.MaxValue}");
            write.WriteBytes_255(content);
        }

        public override void Deserialize(Deserializer reader)
        {
            content = reader.ReadBytes_255();
            Debug.Assert(content != null && content.Length > 0, "!!!!!!!!! Input Cmd len{content?.Length ?? 0} should less then {byte.MaxValue}");
        }
        
    }
}