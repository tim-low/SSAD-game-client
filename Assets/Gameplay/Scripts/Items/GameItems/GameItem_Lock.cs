using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_Lock : GameItem {

        public GameItem_Lock(string description) : base(TypeOfTarget.None, description)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Lock");

            // Get tile color of player who is locking
            int playerColor = refData.PlayerManager.GetGamePlayer(refData.UserToken).Player.Color;

            // Set tile to be locked
            refData.GameBoard.SetLockedTile((GameBoard.TileState)playerColor);
        }
    }

}