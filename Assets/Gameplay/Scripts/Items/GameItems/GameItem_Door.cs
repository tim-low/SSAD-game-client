using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_Door : GameItemTarget {

        public GameItem_Door(string description) : base(TypeOfTarget.Tile, description, true)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Door");

            // Get target tile position
            //int targetId = playerManager.GetGamePlayer(gameItemTargeter.TargetPlayer).PlayerId;
            TilePosition tilePos = refData.VictimTile;
            Vector3 targetPos = refData.GameBoard.GetTilePos(tilePos);

            Debug.Log("Posx: " + tilePos.x + ", y: " + tilePos.y);

            // Set player position at the randomly selected spawn position
            refData.PlayerManager.SetPlayerPosition(refData.UserToken, tilePos, targetPos);

            // Set tile as own player colour
            int playerColor = refData.PlayerManager.GetGamePlayer(refData.UserToken).Player.Color;
            refData.GameBoard.SetTile(tilePos, (GameBoard.TileState)playerColor, refData.GlobalVariables.GetTileColor(playerColor));
        }
    }

}