using System.IO;
using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdCancelSession : OutPacket
    {
        public string SessionCode;
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdCancelSession);
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