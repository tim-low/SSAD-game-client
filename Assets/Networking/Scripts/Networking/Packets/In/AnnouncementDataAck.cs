using System.Collections;
using System.Collections.Generic;

using SuperSad.GameEvents;

namespace SuperSad.Networking
{
    public class AnnouncementDataAck : InPacket
    {
        public Announcement[] Announcements { get; private set; }

        public AnnouncementDataAck(Packet packet) : base(packet)
        {
            int length = Reader.ReadInt32();
            Announcements = new Announcement[length];
            for (int i = 0; i < length; i++)
                Announcements[i] = new Announcement(Reader);
        }
    }
}