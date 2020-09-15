using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Model;
using SuperSad.Gameplay;

namespace SuperSad.Networking
{
    public class TurnPlayerMoveAck : InPacket
    {
        public bool Legal {get; private set;}
        public string Token {get; private set;}
        public TilePosition NewPos {get; private set; }
        public Direction Direction { get; private set; }

        public TurnPlayerMoveAck(Packet packet) : base(packet)
        {
            Legal = Reader.ReadBoolean();
            Token = Reader.ReadUnicodeStatic(44);
            BoardTile temp = new BoardTile(Reader);
            NewPos = new TilePosition(temp.x, temp.y);
            Direction = (Direction)Reader.ReadInt32();
        }
    }

}