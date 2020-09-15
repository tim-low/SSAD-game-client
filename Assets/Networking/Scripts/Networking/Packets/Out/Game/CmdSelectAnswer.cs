using System;
using System.IO;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdSelectAnswer : OutPacket
    {
        public string Token;
        public int SelectedAnswer;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdSelectAnswer);
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
                    sw.Write(SelectedAnswer);
                }
                return ms.ToArray();
            }
        }
    }
}