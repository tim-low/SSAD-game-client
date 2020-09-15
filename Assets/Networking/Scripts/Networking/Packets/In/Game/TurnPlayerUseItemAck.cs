using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Gameplay;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class TurnPlayerUseItemAck : InPacket
    {
        public string Caster;
        public string Victim;
        public ItemType ItemIdentifier;
        public TilePosition Tile;

        public TurnPlayerUseItemAck(Packet packet) : base(packet)
        {
            Caster = Reader.ReadUnicodeStatic(44);
            Victim = Reader.ReadUnicodeStatic(44);
            ItemIdentifier = (ItemType)Reader.ReadInt32();
            BoardTile temp = new BoardTile(Reader);
            Tile = new TilePosition(temp.x, temp.y);
        }
    }

}