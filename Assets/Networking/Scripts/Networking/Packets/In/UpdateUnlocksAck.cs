using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class UpdateUnlocksAck : InPacket
    {
        public bool Success {get; private set;}

        public UpdateUnlocksAck(Packet packet) : base(packet)
        {
            Success = Reader.ReadBoolean();
        }
    }
}
