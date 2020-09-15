using Game_Server.Util;
using System.IO;

namespace SuperSad.Networking
{
    public class CmdTemp9999 : OutPacket {

        private ushort pId;
        public CmdTemp9999(ushort packetId)
        {
            pId = packetId;
        }

        public int Filler = 0;
        public override Packet CreatePacket()
        {
            return base.CreatePacket(pId);
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
                    sw.Write(Filler);
                }
                return ms.ToArray();
            }
        }
    }
}