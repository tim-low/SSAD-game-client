using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class UpdateCharAck : InPacket
    {
        public string Token {get; private set;}

        public UpdateCharAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
        }
    }
}
