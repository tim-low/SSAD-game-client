using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdAuthPlayer : OutPacket
    {
        public string Username;
        public string Password;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdUserAuth);
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Username, 13);
                    sw.WriteTextStatic(Password, 40);
                }
                return ms.ToArray();
            }
        }

    }
}
