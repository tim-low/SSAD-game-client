using System.Collections;

namespace SuperSad.Networking
{
    public class TeacherPlayerJoinedSessionAck : InPacket
    {
        public string StudentName { get; private set; }

        public TeacherPlayerJoinedSessionAck(Packet packet) : base(packet)
        {
            StudentName = Reader.ReadUnicode();
        }
    }
}
