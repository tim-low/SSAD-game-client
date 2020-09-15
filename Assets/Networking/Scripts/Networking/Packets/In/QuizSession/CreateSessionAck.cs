using System.Collections;

namespace SuperSad.Networking
{
    public class CreateSessionAck : InPacket
    {
        public string SessionCode { get; private set; }

        public CreateSessionAck(Packet packet) : base(packet)
        {
            SessionCode = Reader.ReadUnicodeStatic(6);
        }
    }
}
