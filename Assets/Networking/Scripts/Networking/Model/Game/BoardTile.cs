using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game_Server.Util;
using SuperSad.Gameplay;

namespace SuperSad.Model
{
    public class BoardTile : ISerializable {

        public byte x;
        public byte y;
        public byte color;

        public BoardTile(SerializeReader reader)
        {
            x = reader.ReadByte();
            y = reader.ReadByte();
            color = reader.ReadByte();
        }

        public BoardTile(TilePosition tile)
        {
            x = (byte)tile.x;
            y = (byte)tile.y;
            color = 0;
        }

        public void Serialize(SerializeWriter writer)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(color);
        }
    }

}