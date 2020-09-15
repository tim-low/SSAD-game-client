using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Model;

namespace SuperSad.Networking
{
    public class SelectAnswerAck : InPacket
    {
        public int NumPlayers { get; private set; }
        public QuizReward[] Rewards { get; private set; }

        public SelectAnswerAck(Packet packet) : base(packet)
        {
            NumPlayers = Reader.ReadInt32();

            Rewards = new QuizReward[NumPlayers];
            for (int i = 0; i < NumPlayers; i++)
            {
                Rewards[i] = new QuizReward(Reader);
            }
        }
    }

}