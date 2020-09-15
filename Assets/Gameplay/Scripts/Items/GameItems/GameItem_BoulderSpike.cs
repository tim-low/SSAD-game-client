using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_BoulderSpike : GameItemTarget {

        public GameItem_BoulderSpike(string description) : base(TypeOfTarget.Player, description, false)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use BoulderSpike");

            // Get Target Player
            GamePlayer targetPlayer = refData.PlayerManager.GetGamePlayer(refData.VictimToken);

            // Get spawn position of Target Player
            int targetPlayerId = targetPlayer.Player.Color;
            TilePosition spawnTilePos = refData.GlobalVariables.GetSpawnPosition(targetPlayerId);
            Vector3 spawnPos = refData.GameBoard.GetTilePos(spawnTilePos);

            // Set Target Player position at their spawn position
            refData.PlayerManager.SetPlayerPosition(refData.VictimToken, spawnTilePos, spawnPos);
            // Set tile as Target Player's colour
            //refData.GameBoard.SetTile(spawnTilePos, (GameBoard.TileState)targetPlayerId, refData.GlobalVariables.GetTileColor(targetPlayerId));
        }
    }

}