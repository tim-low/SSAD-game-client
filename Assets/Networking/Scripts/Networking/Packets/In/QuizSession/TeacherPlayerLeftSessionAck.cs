using System.Collections;

namespace SuperSad.Networking
{
    public class TeacherPlayerLeftSessionAck : InPacket
    {
        public string StudentName { get; private set; }

        public TeacherPlayerLeftSessionAck(Packet packet) : base(packet)
        {
            StudentName = Reader.ReadUnicode();
        }
    }
}
