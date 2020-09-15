using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdJoinRoom : OutPacket
    {
        public string Token;
        public string RoomId;
        public bool IsLocked;
        public string Password;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdJoinRoom);
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                    sw.WriteTextStatic(RoomId, 36);
                    sw.Write(IsLocked);
                    if (IsLocked) {
                        sw.WriteTextStatic(Password, 40);
                    }               
                }
                return ms.ToArray();
            }
        }

    }
}
