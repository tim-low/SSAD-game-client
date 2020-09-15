using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_Flag : GameItemTarget {

        public GameItem_Flag(string description) : base(TypeOfTarget.Tile, description, false)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Flag");

            TilePosition targetTilePos = refData.VictimTile;

            // Set targeted tile as own player colour
            int playerColor = refData.PlayerManager.GetGamePlayer(refData.UserToken).Player.Color;
            refData.GameBoard.SetTile(targetTilePos, (GameBoard.TileState)playerColor, refData.GlobalVariables.GetTileColor(playerColor));
        }
    }

}