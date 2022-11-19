using System;
using LockStepEngine.Serialization;
using UnityEngine.SocialPlatforms;

namespace LockStepEngine
{
    public interface IBaseMsg
    {
    }

    [Serializable]
    [SelfImplement]
    public class BaseMsg : BaseFormater, IBaseMsg
    {
    }

    #region UDP

    // TODO
    [Serializable]
    [SelfImplement]
    [Udp]
    public class Msg_ReqMissFrame : MutilFrame
    {
        
    }

    #endregion

    #region TCP

    public class IPEndInfo : BaseMsg
    {
        public string Ip;
        public ushort Port;
    }

    public class RoomChangedInfo : BaseMsg
    {
        public int RoomId;
        public byte CurPlayerCount;
    }

    public class RoomInfo : BaseMsg
    {
        public int GameType;
        public int MapId;
        public string Name;
        public byte MaxPlayerCount;

        public int RoomId;
        public byte State;
        public string OwnerName;
        public byte CurPlayerCount;
    }

    public class UserGameInfo : BaseMsg
    {
        public string Name;
        public byte[] Data;
    }

    public class GamePlayerInfo : BaseMsg
    {
        public long UserId;
        public string Account;
        public string LoginHash;
    }

    public class RoomPlayerInfo : BaseMsg
    {
        public long UserId;
        public string Name;
        public byte Status;
    }

    public class RoomChatInfo : BaseMsg
    {
        public byte Channel;
        public long SrcUserId;
        public long DstUserId;
        public byte[] Message;
    }
    
    // TODO: ljm >>> msg
    public class Msg_G2C_GameStartInfo : BaseMsg
    {
        public byte LocalId;
        public byte UserCount;
        public int MapId;
        public int RoomId;
        public int Seed;
        public GameData[] UserInfos;
        public IPEndInfo UdpEnd;
        public IPEndInfo TcpEnd;
        public int SimulationSpeed;

        public override void Serialize(Serializer writer)
        {
            writer.Write(LocalId);
            writer.Write(MapId);
            writer.Write(RoomId);
            writer.Write(Seed);
            writer.Write(UserCount);
            writer.Write(TcpEnd);
            writer.Write(UdpEnd);
            writer.Write(UserInfos);
        }

        public override void Deserialize(Deserializer reader)
        {
            LocalId = reader.ReadByte();
            MapId = reader.ReadInt32();
            RoomId = reader.ReadInt32();
            Seed = reader.ReadInt32();
            SimulationSpeed = reader.ReadInt32();
            UserCount = reader.ReadByte();
            TcpEnd = reader.ReadRef(ref TcpEnd);
            UdpEnd = reader.ReadRef(ref UdpEnd);
            UserInfos = reader.ReadArray(UserInfos);
        }
    }

    #endregion
}