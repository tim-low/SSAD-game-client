using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking
{
    public class GameDespawnPlayerAck : InPacket {

        public string Token { get; private set; }

        public GameDespawnPlayerAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
        }
    }

}