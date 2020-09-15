using System;
using System.Collections.Generic;
//using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class RoomDetailsAck : InPacket
    {
        public Character[] Characters { get; private set; }
        public string OwnerToken { get; private set; }

        public RoomDetailsAck(Packet packet) : base(packet)
        {
            int playerCount = Reader.ReadInt32();

            Characters = new Character[playerCount];
            for(int i = 0; i < playerCount; i++)
            {
                Characters[i] = new Character(Reader);
            }

            OwnerToken = Reader.ReadUnicodeStatic(44);
        }
    }
}
