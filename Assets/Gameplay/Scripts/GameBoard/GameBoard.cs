using System;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Networking.Events;
using SuperSad.Networking;

namespace SuperSad.Gameplay
{
    public class GameBoard : EventListener {
        public enum TileState
        {
            Unassigned = 0,
            Player1 = 1,
            Player2 = 2,
            Player3 = 3,
            Player4 = 4
        }

        [SerializeField]
        private Tilemap3D tilemap;

        //private int rows;
        //private int columns;
        private TileState[,] tileStates;  // -1 for uncoloured, 0-3 for player 1-4 colours

        private Dictionary<TileState, int> lockedTurnsLeft;

        protected override void Init()
        {
            // Initialize locked turns left
            lockedTurnsLeft = new Dictionary<TileState, int>();
            foreach (TileState ts in Enum.GetValues(typeof(TileState)))
            {
                lockedTurnsLeft.Add(ts, 0);
            }
        }

        public void SetLockedTile(TileState tileState)
        {
            lockedTurnsLeft[tileState] = 2; // locked for 2 turns, including current
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to StartPlayerTurnAck & NotifyPlayerTurnAck packet events (start of the next turn)
            notifier.Register(LockForTurnCountdown, Packets.StartPlayerTurnAck);
            notifier.Register(LockForTurnCountdown, Packets.NotifyPlayerTurnAck);
        }

        private void LockForTurnCountdown(Packet packet)
        {
            foreach (TileState ts in Enum.GetValues(typeof(TileState)))
            {
                if (lockedTurnsLeft[ts] > 0)
                {
                    lockedTurnsLeft[ts] -= 1;   // decrement
                }
            }
        }

        private bool IsTileLocked(int x, int y)
        {
            TileState tileToCheck = tileStates[x, y];
            return (lockedTurnsLeft[tileToCheck] > 0);  // tile has some locked turns left
        }

        public void InitGameBoard(int rows, int columns)
        {
            //this.rows = rows;
            //this.columns = columns;
            tilemap.SpawnTiles(rows, columns);

            // initialize Tile States to -1 (uncoloured)
            tileStates = new TileState[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    tileStates[i, j] = TileState.Unassigned;
        }

        public void SetTile(int x, int y, TileState newTileState, Color color)
        {
            if (!IsTileLocked(x, y))    // this tile is not locked; can be overwritten
            {
                tileStates[x, y] = newTileState;
                tilemap.SetTileColor(x, y, color);
            }
        }

        public void SetTile(TilePosition tilePos, TileState newTileState, Color color)
        {
            SetTile(tilePos.x, tilePos.y, newTileState, color);
        }

        public TileState GetTileState(TilePosition tilePos)
        {
            return GetTileState(tilePos.x, tilePos.y);
        }
        public TileState GetTileState(int x, int y)
        {
            return tileStates[x, y];
        }

        public Vector3 GetTilePos(int x, int y)
        {
            return tilemap.CalcTilePosition(x, y);
        }

        public Vector3 GetTilePos(TilePosition pos)
        {
            return GetTilePos(pos.x, pos.y);
        }

        public GameTile GetGameTile(int x, int y)
        {
            return tilemap.GetTileObject(x, y);
        }
    }

}