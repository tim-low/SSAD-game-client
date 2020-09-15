using System.Collections;
using System.Collections.Generic;

namespace SuperSad.Networking
{
    public class LeaveSessionAck : InPacket
    {
        public string Token { get; private set; }

        public LeaveSessionAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
        }
    }
}