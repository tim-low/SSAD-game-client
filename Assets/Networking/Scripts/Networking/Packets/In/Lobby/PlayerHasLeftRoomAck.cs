using System;
//using System.Numerics;
//using SampleClient.Object;

using SuperSad.Model;

namespace SuperSad.Networking
{
    public class PlayerHasLeftRoomAck : InPacket
    {
        public Character Character {get; private set; }
        public bool HasOwnerChanged { get; private set; }
        public string OwnerToken { get; private set; }

        public PlayerHasLeftRoomAck(Packet packet) : base(packet)
        {
            Character = new Character(Reader);

            HasOwnerChanged = Reader.ReadBoolean();
            if (HasOwnerChanged)
                OwnerToken = Reader.ReadUnicodeStatic(44);
        }
    }
}
