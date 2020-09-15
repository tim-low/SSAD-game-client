using System.IO;
using System.Collections.Generic;
using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdAnnouncementData : OutPacket
    {
        public List<int> AnnouncementIds;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdAnnouncementData);
        }

        public override int ExpectedSize()
        {
            return 4;
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.Write(AnnouncementIds.Count);
                    foreach (int id in AnnouncementIds)
                        sw.Write(id);
                }
                return ms.ToArray();
            }
        }
    }

}