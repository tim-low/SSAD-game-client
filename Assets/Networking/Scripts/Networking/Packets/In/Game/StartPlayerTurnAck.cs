using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking
{
    public class StartPlayerTurnAck : InPacket
    {
        public string Token { get; private set; }
        public int Duration {get; private set;}

        public StartPlayerTurnAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
            Duration = Reader.ReadInt32();
        }
    }

}