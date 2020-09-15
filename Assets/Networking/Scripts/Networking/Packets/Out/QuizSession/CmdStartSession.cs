using System.IO;
using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdStartSession : OutPacket
    {
        public string SessionCode;
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdStartSession);
        }

        public override int ExpectedSize()
        {
            return 6;
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(SessionCode, 6);
                }
                return ms.ToArray();
            }
        }
    }
}