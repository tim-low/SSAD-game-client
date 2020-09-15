using System;

using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class ReadyStatusAck : InPacket
    {
        public string Token {get; private set;}
        public bool IsReady {get; private set;}

        public ReadyStatusAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
            IsReady = Reader.ReadBoolean();
        }
    }
}
