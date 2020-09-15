using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboards
{
    //displays text on screen
    public class HallOfFameEntryUI : MonoBehaviour
    {

        [SerializeField] private Text entryNameText;
        [SerializeField] private Text entryDateText;

        public void Initialise(HallOfFameEntryData hallOfFameEntryData)
        {
            entryNameText.text = hallOfFameEntryData.entryUsername;
            entryDateText.text = hallOfFameEntryData.entryDateTime.ToString();
        }
    }
}
