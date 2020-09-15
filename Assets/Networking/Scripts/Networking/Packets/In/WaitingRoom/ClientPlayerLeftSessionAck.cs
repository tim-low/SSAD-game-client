using System.Collections;
using System.Collections.Generic;

namespace SuperSad.Networking
{
    public class ClientPlayerLeftSessionAck : InPacket
    {
        public string Token { get; private set; }

        public ClientPlayerLeftSessionAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
        }
    }
}