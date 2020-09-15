using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Networking;

namespace SuperSad.Gameplay
{
    public class DirectionButton : MonoBehaviour {

        [SerializeField]
        private Direction direction;
        [SerializeField]
        private GameVariables globalVariables;

        public void SendMoveCommand()
        {
            if (globalVariables.MoveCommand(direction))
            {
                // temporarily disable buttons?
                TurnAction.Instance.EnableActionableUI(false);

                Debug.Log("Move");
            }
            else
                Debug.Log("Move illegal");
        }
    }

}