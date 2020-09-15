using System;

namespace SuperSad.Networking
{
    public class UserAuthAck : InPacket
    {
        //public readonly bool Success;
        public readonly string Message;

        public UserAuthAck(Packet packet) : base(packet)
        {
            //Success = Reader.ReadBoolean();
            Message = Reader.ReadUnicodeStatic(44);
        }
    }
}
