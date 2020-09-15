using System;
//using System.Numerics;
//using SampleClient.Object;

using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class JoinRoomAck : InPacket
    {
        public Room Room {get; private set;}

        public JoinRoomAck(Packet packet) : base(packet)
        {
            Room = new Room(Reader);
        }
    }
}
