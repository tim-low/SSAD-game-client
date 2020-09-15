using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class PlayerTarget : ItemTarget {

        [SerializeField]
        private GamePlayer gamePlayer;

        protected override object GetComponentData()
        {
            return gamePlayer.Player.Token;
        }

        // Use this for initialization
        void Awake()
        {

            if (gamePlayer == null)
                gamePlayer = GetComponent<GamePlayer>();

        }
    }

}