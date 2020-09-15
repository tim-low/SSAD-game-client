using Game_Server.Util;
using System.IO;

namespace SuperSad.Networking
{
    public class CmdSendKaomoji : OutPacket
    {
        public string Token;
        public int KaomojiId;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdSendKaomoji);
        }

        public override int ExpectedSize()
        {
            return 48;
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                    sw.Write(KaomojiId);
                }
                return ms.ToArray();
            }
        }
    }
}
