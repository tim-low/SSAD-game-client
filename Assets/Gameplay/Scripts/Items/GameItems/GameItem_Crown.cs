using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_Crown : GameItem {

        public GameItem_Crown(string description) : base(TypeOfTarget.None, description)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Crown");
            TilePosition position = refData.PlayerManager.GetGamePlayer(refData.UserToken).Position;
            int playerColorId = refData.PlayerManager.GetGamePlayer(refData.UserToken).Player.Color;

            // assign all 8 tiles surrounding player to the player as well as current tile
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

            Color playerColor = refData.GlobalVariables.GetTileColor(playerColorId);
            GameBoard.TileState playerTileState = (GameBoard.TileState)playerColorId;
            foreach (TilePosition offset in surroundingOffsets)
            {
                TilePosition newPos = position + offset;
                if (newPos.x >= 0 && newPos.x < refData.GlobalVariables.XLength &&
                    newPos.y >= 0 && newPos.y < refData.GlobalVariables.YLength)
                    refData.GameBoard.SetTile(newPos, playerTileState, playerColor);
            }
        }
    }

}