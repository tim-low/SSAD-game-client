using System.IO;
using Game_Server.Util;

namespace SuperSad.Networking {
    public abstract class InPacket {

        protected SerializeReader Reader;

        public InPacket(Packet packet)
        {
            Reader = new SerializeReader(new MemoryStream(packet.Buffer));
        }
    }
}