using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Model;

namespace SuperSad.Networking
{
    public class InitializeCycleAck : InPacket
    {
        public Player[] PlayerSequence {get; private set;}

        //private const int maxPlayers = 4;

        public InitializeCycleAck(Packet packet) : base(packet)
        {
            int size = Reader.ReadInt32();
            PlayerSequence = new Player[size];
            for(int i = 0; i < size; i++)
            {
                PlayerSequence[i] = new Player(Reader);
            }
        }
    }

}