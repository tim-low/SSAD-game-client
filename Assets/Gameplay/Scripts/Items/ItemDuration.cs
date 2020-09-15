using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Gameplay
{
    public class ItemDuration {

        private int countdown;
        public string VictimPlayer { get; private set; }

        public ItemDuration(int countdown, string victimPlayer = "")
        {
            this.countdown = countdown;
            VictimPlayer = victimPlayer;
        }

        public bool IsCountdownUp()
        {
            return (countdown == 0);
        }

        public void ReduceCountdown()
        {
            if (countdown > 0)
                countdown -= 1;
        }
    }

}