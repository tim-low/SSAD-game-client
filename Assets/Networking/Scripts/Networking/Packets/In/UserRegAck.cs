using System;
using System.IO;

namespace SuperSad.Networking
{
    public class UserRegAck : InPacket
    {
        public readonly string Token;

        public UserRegAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
        }
    }
}
