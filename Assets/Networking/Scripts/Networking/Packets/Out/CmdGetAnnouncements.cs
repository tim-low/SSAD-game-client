using System.IO;
using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdGetAnnouncements : OutPacket
    {
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdGetAnnouncements);
        }

        public override int ExpectedSize()
        {
            return 4;
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    int filler = 0;
                    sw.Write(filler);
                }
                return ms.ToArray();
            }
        }
    }

}