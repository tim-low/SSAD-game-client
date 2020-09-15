using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    // Sent to player when they leave room
    public class CmdLeaveLobby : OutPacket
    {
        public string Token;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdLeaveLobby);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                }
                return ms.ToArray();
            }
        }
    }
}