using System.Collections;

namespace SuperSad.Networking
{
    public class StartSessionAck : InPacket
    {
        public bool Success { get; private set; }

        public StartSessionAck(Packet packet) : base(packet)
        {
            Success = Reader.ReadBoolean();
        }
    }
}
