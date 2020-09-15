using System;

using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class UpdateRoomStatusAck : InPacket
    {
        public string RoomId {get; private set;}
        public bool IsInGame {get; private set;}

        public UpdateRoomStatusAck(Packet packet) : base(packet)
        {
            RoomId = Reader.ReadUnicodeStatic(36);
            IsInGame = Reader.ReadBoolean();
        }
    }
}
