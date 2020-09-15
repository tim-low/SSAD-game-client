using Game_Server.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SuperSad.Networking
{
    public class SendKaomojiAck : InPacket
    {
        public string Token {get; private set;}
        public int KaomojiId {get; private set;}

        public SendKaomojiAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
            KaomojiId = Reader.ReadInt32();
        }
    }
}
