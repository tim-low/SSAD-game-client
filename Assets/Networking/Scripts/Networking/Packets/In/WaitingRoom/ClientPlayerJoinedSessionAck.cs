using SuperSad.Model;

namespace SuperSad.Networking
{
    public class ClientPlayerJoinedSessionAck : InPacket
    {
        public WaitingRoomUser User { get; private set; }

        public ClientPlayerJoinedSessionAck(Packet packet) : base(packet)
        {
            User = new WaitingRoomUser(Reader);
        }
    }
}