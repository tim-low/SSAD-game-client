using System.Collections;
using System.Collections.Generic;
using Game_Server.Util;
using System;

//a simple struct with two variables. 
namespace Leaderboards{

    //data can be saved to computer
    [Serializable]
    public class LeaderboardEntryData
    {
        //a single entry
        public string entryUsername { get; private set; }
        public int entryScore { get; private set; }

        public LeaderboardEntryData(SerializeReader reader)
        {
            entryUsername = reader.ReadUnicodeStatic(13);
            entryScore = reader.ReadInt32();
        }
    }
}


