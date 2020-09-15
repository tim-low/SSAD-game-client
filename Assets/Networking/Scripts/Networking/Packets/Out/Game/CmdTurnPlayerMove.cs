using System;
using System.IO;

using Game_Server.Util;
using SuperSad.Gameplay;

namespace SuperSad.Networking
{
    public class CmdTurnPlayerMove : OutPacket
    {
        public string Token;
        public Direction Direction;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdTurnPlayerMove);
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
                    sw.Write((int)Direction);
                }
                return ms.ToArray();
            }
        }

    }
}