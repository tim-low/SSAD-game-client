using System.Collections;
using System.Collections.Generic;

namespace SuperSad.Networking
{
    public class GetAnnouncementsAck : InPacket
    {
        public int[] AnnouncementIds { get; private set; }

        public GetAnnouncementsAck(Packet packet) : base(packet)
        {
            int length = Reader.ReadInt32();
            if (length == 0)    // no announcements available
                AnnouncementIds = null;
            else
            {
                AnnouncementIds = new int[length];
                for (int i = 0; i < length; i++)
                    AnnouncementIds[i] = Reader.ReadInt32();
            }
        }
    }
}