using System;

using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class CanStartAck : InPacket
    {
        public bool Success {get; private set;}

        public CanStartAck(Packet packet) : base(packet)
        {
            Success = Reader.ReadBoolean();
        }
    }
}
