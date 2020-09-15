using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Leaderboards
{

    [Serializable]
    public class LeaderboardSaveData
    {
        //initialize list - if text file doesn't exists, create an empty list of high scores.
        public List<LeaderboardEntryData> highscores = new List<LeaderboardEntryData>();
    }

}
