using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_Coin : GameItem {

        public GameItem_Coin(string description) : base(TypeOfTarget.None, description)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Coin");

            // Get new number of steps - +2 
            GamePlayer casterPlayer = refData.PlayerManager.GetGamePlayer(refData.UserToken);
            int newSteps = casterPlayer.Player.StepsLeft + 2;

            // Increase Steps Left count
            casterPlayer.Player.SetSteps(newSteps);
            // Update number steps UI
            refData.PlayerManager.UpdateNumberStepsText(newSteps);
        }
    }

}