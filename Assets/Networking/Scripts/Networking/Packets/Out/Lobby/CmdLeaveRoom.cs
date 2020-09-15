using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdLeaveRoom : OutPacket
    {
        public string Token;
        public string RoomId;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdLeaveRoom);
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                    sw.WriteTextStatic(RoomId, 36);
                }
                return ms.ToArray();
            }
        }

    }
}
