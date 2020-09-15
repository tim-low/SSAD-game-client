using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class GameItem_GroundSpike : GameItemTarget {

        public GameItem_GroundSpike(string description) : base(TypeOfTarget.Tile, description, true)
        {
        }

        public override void UseEffect(StateReference refData)
        {
            Debug.Log("Use GroundSpike");
        }
    }

}