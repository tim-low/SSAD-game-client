using System.IO;
using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdCreateSession : OutPacket
    {
        public int QuizId;
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdCreateSession);
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
                    sw.Write(QuizId);
                }
                return ms.ToArray();
            }
        }
    }
}