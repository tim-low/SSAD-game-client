using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking
{
    public class NotifyPlayerTurnAck : InPacket {

        public string Token { get; private set; }
        public int Duration { get; private set; }

        public NotifyPlayerTurnAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
            Duration = Reader.ReadInt32();
        }
    }

}