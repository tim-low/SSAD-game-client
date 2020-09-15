using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperSad.Gameplay;

namespace SuperSad.Gameplay
{
    public class GameTile : ItemTarget {

        public TilePosition TilePosition
        {
            get; private set;
        }

        public void SetTilePosition(int x, int y)
        {
            TilePosition = new TilePosition(x, y);
        }

        protected override object GetComponentData()
        {
            return TilePosition;
        }
    }

}