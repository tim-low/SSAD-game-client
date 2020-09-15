using System;
//using System.Numerics;
//using SampleClient.Object;

using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class LeaveLobbyAck : InPacket
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        public LeaveLobbyAck(Packet packet) : base(packet)
        {
            Success = Reader.ReadBoolean();
            Message = Reader.ReadUnicodeStatic(44);
        }
    }
}