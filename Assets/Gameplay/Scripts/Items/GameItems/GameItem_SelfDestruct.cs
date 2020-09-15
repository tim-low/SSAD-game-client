using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_SelfDestruct : GameItem {

        public GameItem_SelfDestruct(string description) : base(TypeOfTarget.None, description)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use SelfDestruct");
            TilePosition position = refData.PlayerManager.GetGamePlayer(refData.UserToken).Position;

            // unassign all 8 tiles surrounding player as well as current tile
            TilePosition[] surroundingOffsets = {
                new TilePosition(0, 0),
                new TilePosition(-1, -1),
                new TilePosition(-1, 0),
                new TilePosition(-1, 1),
                new TilePosition(0, 1),
                new TilePosition(1, 1),
                new TilePosition(1, 0),
                new TilePosition(1, -1),
                new TilePosition(0, -1),
            };

            Color unassignedColor = refData.GlobalVariables.GetTileColor((int)GameBoard.TileState.Unassigned);
            foreach (TilePosition offset in surroundingOffsets)
            {
                TilePosition newPos = position + offset;
                if (newPos.x >= 0 && newPos.x < refData.GlobalVariables.XLength &&
                    newPos.y >= 0 && newPos.y < refData.GlobalVariables.YLength)
                    refData.GameBoard.SetTile(newPos, GameBoard.TileState.Unassigned, unassignedColor);
            }
        }
    }

}