using System.IO;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdUserLogout : OutPacket
    {
        private const int Filler = 0;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdUserLogout);
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