using System.Collections;
using System.IO;
using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdLeaderboard : OutPacket {

        public string Token;    // 44 bytes
        public int PageNumber;  // 4 bytes
        public int NumEntries;  // 4 bytes

        public int LifeCycleStage;  // 4 bytes
        

        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdLeaderboard);
        }

        public override int ExpectedSize()
        {
            //return 52;
            return 56;
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                    sw.Write(PageNumber);
                    sw.Write(NumEntries);

                    sw.Write(LifeCycleStage);
                }
                return ms.ToArray();
            }
        }

    }

}