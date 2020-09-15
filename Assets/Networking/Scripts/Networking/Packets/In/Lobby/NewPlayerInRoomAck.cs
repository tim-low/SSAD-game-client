using System;
//using System.Numerics;
//using SampleClient.Object;

using SuperSad.Model;

namespace SuperSad.Networking
{
    public class NewPlayerInRoomAck : InPacket
    {
        public Character Character {get; private set;}

        public NewPlayerInRoomAck(Packet packet) : base(packet)
        {
            Character = new Character(Reader);
        }
    }
}
