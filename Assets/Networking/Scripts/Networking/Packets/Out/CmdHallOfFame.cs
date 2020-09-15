using System.Collections;
using System.IO;
using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdHallOfFame : OutPacket
    {
        
        public int PageNumber;  // 4 bytes
        public int NumEntries;  // 4 bytes
        


        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdHallOfFame);
        }

        public override int ExpectedSize()
        {
            return 8;
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.Write(PageNumber);
                    sw.Write(NumEntries);
                }
                return ms.ToArray();
            }
        }

    }
}


