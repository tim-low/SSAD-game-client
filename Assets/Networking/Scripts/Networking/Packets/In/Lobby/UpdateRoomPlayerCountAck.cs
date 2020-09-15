using System;
//using System.Numerics;
//using SampleClient.Object;

using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class UpdateRoomPlayerCountAck : InPacket
    {
        public string RoomId {get; private set;}
        public int NumOfUser {get; private set;}

        public UpdateRoomPlayerCountAck(Packet packet) : base(packet)
        {
            RoomId = Reader.ReadUnicodeStatic(36);
            NumOfUser = Reader.ReadInt32();
        }
    }
}
