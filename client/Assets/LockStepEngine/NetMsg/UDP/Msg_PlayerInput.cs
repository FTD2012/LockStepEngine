using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices.WindowsRuntime;
using LockStepEngine.Serialization;
using LockStepEngine.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace LockStepEngine
{
    [Serializable]
    [SelfImplement]
    [Udp]
    public class Msg_PlayerInput : BaseMsg
    {
        public bool IsMiss;
        public byte ActorId;
        public int Tick;
        public InputCmd[] InputCommands;
#if DEBUG_FRAME_DELAY
        public float timeSinceStartUp;
#endif

        public Msg_PlayerInput()
        {
            
        }

        public Msg_PlayerInput(int tick, byte actorId, List<InputCmd> inputList = null)
        {
            Tick = tick;
            ActorId = actorId;
            if (inputList != null && inputList.Count > 0)
            {
                InputCommands = inputList.ToArray();
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Msg_PlayerInput);
        }

        public bool Equals(Msg_PlayerInput other)
        {
            if (other == null)
            {
                return false;
            }

            if (Tick != other.Tick)
            {
                return false;
            }

            return InputCommands.EqualsEx(other.InputCommands);
        }

        public override int GetHashCode()
        {
            return (ActorId << 24 & Tick);
        }

        public override void Serialize(Serializer writer)
        {
#if DEBUG_FRAME_DELAY
            writer.Write(timeSinceStartUp);
#endif
            writer.Write(IsMiss);
            writer.Write(ActorId);
            writer.Write(Tick);

            var count = InputCommands?.Length ?? 0;
            writer.Write(count);
            for (int i = 0; i < count; i++)
            {
                InputCommands[i].Serialize(writer);
            }
        }

        public override void Deserialize(Deserializer reader)
        {
#if DEBUG_FRAME_DELAY
            timeSinceStartUp = reader.ReadSingle();
#endif
            IsMiss = reader.ReadBoolean();
            ActorId = reader.ReadByte();
            Tick = reader.ReadInt32();
            int count = reader.ReadByte();
            if (count == 0)
            {
                InputCommands = null;
                return;
            }

            InputCommands = new InputCmd[count];
            for (int i = 0; i < count; i++)
            {
                InputCommands[i] = reader.Parse<InputCmd>();
            }
        }
    }
}