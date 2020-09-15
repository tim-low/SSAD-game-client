using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperSad.Gameplay
{
    public class RankListItem : MonoBehaviour {

        [SerializeField]
        private Text rankNumberText;
        [SerializeField]
        private Text usernameText;
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Text qnsCorrectText;
        [SerializeField]
        [Tooltip("For highlighting the local client's record")]
        private GameObject rowHighlight;

        public void SetRank(int rank, Color color)
        {
            rankNumberText.text = rank.ToString();
            rankNumberText.color = color;
        }
        public void SetUsername(string username, Color color)
        {
            usernameText.text = username;
            usernameText.color = color;
        }
        public void SetScore(int score, Color color)
        {
            scoreText.text = score.ToString();
            scoreText.color = color;
        }
        public void SetQuestionsCorrect(int numCorrect, Color color)
        {
            qnsCorrectText.text = numCorrect.ToString();
            qnsCorrectText.color = color;
        }

        /// <summary>
        /// This rank list item corresponds to this local client.
        /// </summary>
        /// <param name="isLocalClient"></param>
        public void SetIsLocalClient(bool isLocalClient)
        {
            rowHighlight.SetActive(isLocalClient);
        }
    }

}