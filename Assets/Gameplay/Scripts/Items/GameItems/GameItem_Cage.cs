using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_Cage : GameItemTarget {

        public GameItem_Cage(string description) : base(TypeOfTarget.Player, description, false)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use Cage");

            // Lock Player
            refData.TurnManager.LockPlayer(refData.VictimToken);
        }
    }

}