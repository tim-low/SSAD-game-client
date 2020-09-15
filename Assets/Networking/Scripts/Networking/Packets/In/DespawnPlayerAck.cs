using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking
{
    public class DespawnPlayerAck : InPacket
    {
        public string Token;

        public DespawnPlayerAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
        }
    }

}