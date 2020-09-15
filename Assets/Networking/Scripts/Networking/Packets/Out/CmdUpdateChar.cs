using System;
using System.IO;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdUpdateChar : OutPacket
    {
        public string Token;
        public byte Head;
        public byte Shirt;
        public byte Pant;
        public byte Shoe;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdUpdateChar);
        }

        public override int ExpectedSize()
        {
            return 48;
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                    sw.Write(Head);
                    sw.Write(Shirt);
                    sw.Write(Pant);
                    sw.Write(Shoe);
                }
                return ms.ToArray();
            }
        }

    }
}
