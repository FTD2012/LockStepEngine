using System;
using LockStepEngine.Serialization;

namespace LockStepEngine
{
    public class PlayerPing : BaseMsg
    {
        public byte localId;
        public long sendTimestamp;

        public override void Serialize(Serializer writer)
        {
            writer.Write(localId);
            writer.Write(sendTimestamp);
        }

        public override void Deserialize(Deserializer reader)
        {
            localId = reader.ReadByte();
            sendTimestamp = reader.ReadInt64();
        }
    }

    [Serializable]
    [SelfImplement]
    [Udp]
    public class Msg_C2GPlayerPing : PlayerPing
    {
        
    }

    [Serializable]
    [SelfImplement]
    [Udp]
    public class Msg_G2C_PlayerPing : PlayerPing
    {
        public long timeSinceServerStart;
        public override void Serialize(Serializer writer)
        {
            base.Serialize(writer);
            writer.Write(timeSinceServerStart);
        }

        public override void Deserialize(Deserializer reader)
        {
            base.Deserialize(reader);
            timeSinceServerStart = reader.ReadInt64();
        }
    }
    
    public class ServerFrame : BaseMsg
    {
        public byte[] inputDatas;
        public int tick;
        public Msg_PlayerInput[] inputs;

        private byte[] serverInputs;

        public Msg_PlayerInput[] Inputs
        {
            get
            {
                return inputs;
            }
            set
            {
                inputs = value;
                inputDatas = null;
            }
        }

        public void BeforeSerialize()
        {
            if (inputDatas != null)
            {
                return;
            }

            var writer = new Serializer();
            var inputLen = (byte)(Inputs?.Length ?? 0);
            writer.Write(inputLen);
            for (byte i = 0; i < inputLen; i++)
            {
                var cmds = Inputs[i].InputCommands;
                var len = (byte)(cmds?.Length ?? 0);
                writer.Write(len);
                for (int cmdIdx = 0; cmdIdx < len; cmdIdx++)
                {
                    cmds[cmdIdx].Serialize(writer);
                }
            }

            writer.WriteBytes_255(serverInputs);
            inputDatas = writer.CopyData();
        }

        public void AfterDeserialize()
        {
            var reader = new Deserializer(inputDatas);
            var inputLen = reader.ReadByte();
            inputs = new Msg_PlayerInput[inputLen];
            for (byte i = 0; i < inputLen; i++)
            {
                var input = new Msg_PlayerInput();
                input.Tick = tick;
                input.ActorId = i;
                inputs[i] = input;
                
                var len = reader.ReadByte();
                if (len == 0)
                {
                    input.InputCommands = null;
                    continue;
                }

                input.InputCommands = new InputCmd[len];
                for (int cmdIdx = 0; cmdIdx < len; cmdIdx++)
                {
                    var cmd = new InputCmd();
                    cmd.Deserialize(reader);
                    input.InputCommands[cmdIdx] = cmd;
                }
            }

            serverInputs = reader.ReadBytes_255();
        }
        
        public override void Serialize(Serializer writer)
        {
            BeforeSerialize();
            writer.Write(tick);
            writer.Write(inputDatas);
        }

        public override void Deserialize(Deserializer reader)
        {
            tick = reader.ReadInt32();
            inputDatas = reader.ReadBytes();
            AfterDeserialize();
        }
        
        public override string ToString()
        {
            var count = (inputDatas == null) ? 0 : inputDatas.Length;
            return "t:{tick} " + $"inputNum:{count}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var frame = obj as ServerFrame;
            return Equals(frame);
        }

        public override int GetHashCode()
        {
            return tick;
        }

        public bool Equals(ServerFrame frame)
        {
            if (frame == null) return false;
            if (tick != frame.tick) return false;
            BeforeSerialize();
            frame.BeforeSerialize();
            return inputDatas.EqualsEx(frame.inputDatas);
        }
    }
}