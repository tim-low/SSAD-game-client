using System.Collections;
using System.Collections.Generic;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class JoinSessionAck : InPacket
    {
        public string QuizName { get; private set; }
        public WaitingRoomUser[] WaitingRoomUsers { get; private set; }

        public JoinSessionAck(Packet packet) : base(packet)
        {
            QuizName = Reader.ReadUnicode();

            int length = Reader.ReadInt32();
            WaitingRoomUsers = new WaitingRoomUser[length];
            for (int i = 0; i < length; i++)
            {
                WaitingRoomUsers[i] = new WaitingRoomUser(Reader);
            }
        }
    }
}