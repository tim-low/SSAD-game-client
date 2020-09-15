using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdWorldSelect : OutPacket
    {
        public string Token;
        public int LobbyId;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdWorldSelect);
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                    sw.Write(LobbyId);
                }
                return ms.ToArray();
            }
        }

    }
}
