using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking
{
    public class EndPlayerTurnAck : InPacket {

        public int Filler { get; private set; }

        public EndPlayerTurnAck(Packet packet) : base(packet)
        {
            Filler = Reader.ReadInt32();
        }
    }

}