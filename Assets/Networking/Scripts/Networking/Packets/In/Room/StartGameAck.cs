using System;

using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class StartGameAck : InPacket
    {
        public bool Success {get; private set;}

        public StartGameAck(Packet packet) : base(packet)
        {
            Success = Reader.ReadBoolean();
        }
    }
}
