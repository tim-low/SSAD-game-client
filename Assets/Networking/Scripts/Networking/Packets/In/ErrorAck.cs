using System;

namespace SuperSad.Networking
{
    public class ErrorAck : InPacket
    {
        public string Message;
        public ErrorAck(Packet packet) : base(packet)
        {
            Message = Reader.ReadUnicodePrefixed();
        }

        /* OutPacket
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ErrorAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteText(Message);
                }
                return ms.ToArray();
            }
        }
        */
    }
}
