using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_WizardHat : GameItemTarget {

        public GameItem_WizardHat(string description) : base(TypeOfTarget.Player, description, false)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Wizard");

            // Get Players
            GamePlayer userPlayer = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            // Get Target Player
            GamePlayer targetPlayer = refData.PlayerManager.GetGamePlayer(refData.VictimToken);

            // Get user player's new position
            TilePosition userNewTilePos = new TilePosition(targetPlayer.Position);
            Vector3 userNewPos = refData.GameBoard.GetTilePos(userNewTilePos);
            // Get target player's new position
            TilePosition targetNewTilePos = new TilePosition(userPlayer.Position);
            Vector3 targetNewPos = refData.GameBoard.GetTilePos(targetNewTilePos);

            // Set user player at target player's position
            refData.PlayerManager.SetPlayerPosition(refData.UserToken, userNewTilePos, userNewPos);
            // Set tile colour for user player
            int playerColor = userPlayer.Player.Color;
            refData.GameBoard.SetTile(userNewTilePos, (GameBoard.TileState)playerColor, refData.GlobalVariables.GetTileColor(playerColor));

            // Set target player at user player's position
            refData.PlayerManager.SetPlayerPosition(refData.VictimToken, targetNewTilePos, targetNewPos);
            // Set tile colour for target player
            //int targetColor = targetPlayer.Player.Color;
            //refData.GameBoard.SetTile(targetNewTilePos, (GameBoard.TileState)targetColor, refData.GlobalVariables.GetTileColor(targetColor));
        }
    }

}