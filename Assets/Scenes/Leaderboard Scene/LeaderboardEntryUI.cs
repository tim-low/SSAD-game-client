using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboards
{
    //displays text on screen
    public class LeaderboardEntryUI : MonoBehaviour
    {

        [SerializeField] private Text entryNameRank;
        [SerializeField] private Text entryNameText;
        [SerializeField] private Text entryNameScore;

        public void Initialise(LeaderboardEntryData leaderboardEntryData, int rank)
        {
            entryNameRank.text = rank.ToString();
            entryNameText.text = leaderboardEntryData.entryUsername;
            entryNameScore.text = leaderboardEntryData.entryScore.ToString();

        }
    }
}

