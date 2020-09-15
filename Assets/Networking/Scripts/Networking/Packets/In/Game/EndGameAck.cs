using System.Collections;
using System.Collections.Generic;

using SuperSad.Model;

namespace SuperSad.Networking
{
    public class EndGameAck : InPacket {

        public GameScore[] GameScores { get; private set; }

        public EndGameAck(Packet packet) : base(packet)
        {
            int size = Reader.ReadInt32();
            GameScores = new GameScore[size];
            for (int i = 0; i < size; i++)
            {
                GameScores[i] = new GameScore(Reader);
            }
        }
    }

}