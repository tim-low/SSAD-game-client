using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Model;

namespace SuperSad.Networking
{
    public class InitializeGameAck : InPacket
    {
        //public List<BoardTile> Board {get; private set;}
        public Player[] PlayerSequence {get; private set;}
        public Attributes[] PlayerAttributes { get; private set; }
        public byte NumberTurns { get; private set; }

        public InitializeGameAck(Packet packet) : base(packet)
        {
            /*Board = new List<BoardTile>();
            int boardSize = packet.Reader.ReadInt32();
            for(int i = 0; i < boardSize; i++)
            {
                BoardTile tile = new BoardTile(packet.Reader);
                Board.Add(tile);
            }*/
            int playerSize = Reader.ReadInt32();
            PlayerSequence = new Player[playerSize];
            PlayerAttributes = new Attributes[playerSize];
            for (int i = 0; i < playerSize; i++)
            {
                PlayerSequence[i] = new Player(Reader);
                PlayerAttributes[i] = new Attributes(Reader);
            }

            NumberTurns = Reader.ReadByte();
        }
    }

}