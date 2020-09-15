using System.Collections;

namespace SuperSad.Networking
{
    public class CancelSessionAck : InPacket
    {
        public bool Cancelled { get; private set; }

        public CancelSessionAck(Packet packet) : base(packet)
        {
            Cancelled = Reader.ReadBoolean();
        }
    }
}