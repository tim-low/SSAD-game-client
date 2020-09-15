using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [System.Serializable]
    public struct TilePosition
    {
        public int x;
        public int y;

        public TilePosition(TilePosition other)
        {
            this.x = other.x;
            this.y = other.y;
        }
        public TilePosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(TilePosition a, TilePosition b)
        {
            return (a.x == b.x) && (a.y == b.y);
        }
        public static bool operator !=(TilePosition a, TilePosition b)
        {
            return (a.x != b.x) || (a.y != b.y);
        }
        public override bool Equals(object obj)
        {
            TilePosition other = (TilePosition)obj;
            return this == other;
        }

        public static TilePosition operator +(TilePosition a, TilePosition b)
        {
            int x = a.x + b.x;
            int y = a.y + b.y;

            return new TilePosition(x, y);
        }

        public static TilePosition operator +(TilePosition tilePos, Direction dir)
        {
            TilePosition result = new TilePosition(tilePos);

            switch (dir)
            {
                case Direction.Up:
                    result.y += 1;
                    break;

                case Direction.Down:
                    result.y -= 1;
                    break;

                case Direction.Left:
                    result.x -= 1;
                    break;

                case Direction.Right:
                    result.x += 1;
                    break;
            }

            return result;
        }
    }

}