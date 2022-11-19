using System.Collections.Generic;

namespace LockStepEngine
{
    public interface INetworkService : IService
    {
        void SendGameEvent(byte[] data);
        void SendPing(byte localId, long timestamp);
        void SendInput(Msg_PlayerInput msg);
        void SendHashCodeList(int startTick, List<int> hashCodeList, int startIndex, int count);
        void SendMissFrameReq(int missFrameTick);
        void SendMissFrameReqAck(int missFrameTick);
    }
}