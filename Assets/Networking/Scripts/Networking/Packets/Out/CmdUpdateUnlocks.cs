using System;
using System.IO;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdUpdateUnlocks : OutPacket
    {
        public string Token;
        public byte Head;
        public byte Shirt;
        public byte Pants;
        public byte Shoes;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdUpdateUnlocks);
        }

        public override int ExpectedSize()
        {
            return 44 + (4*1);
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
                    sw.Write(Pants);
                    sw.Write(Shoes);
                }
                return ms.ToArray();
            }
        }

    }
}
