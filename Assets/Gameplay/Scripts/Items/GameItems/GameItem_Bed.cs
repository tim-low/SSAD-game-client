using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_Bed : GameItem {

        public GameItem_Bed(string description) : base(TypeOfTarget.None, description)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Bed");

            // Get own spawn position
            int playerColor = refData.PlayerManager.GetGamePlayer(refData.UserToken).Player.Color;
            TilePosition spawnTilePos = refData.GlobalVariables.GetSpawnPosition(playerColor);
            Vector3 spawnPos = refData.GameBoard.GetTilePos(spawnTilePos);

            Debug.Log("Posx: " + spawnTilePos.x + ", y: " + spawnTilePos.y);

            // Set player position at own spawn position
            refData.PlayerManager.SetPlayerPosition(refData.UserToken, spawnTilePos, spawnPos);

            // Set tile as own player colour
            refData.GameBoard.SetTile(spawnTilePos, (GameBoard.TileState)playerColor, refData.GlobalVariables.GetTileColor(playerColor));
        }
    }

}