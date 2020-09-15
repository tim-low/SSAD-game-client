using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperSad.Gameplay
{
    public class TurnData {
        public TurnData(string token, string username, Color color)
        {
            Token = token;
            Username = username;
            PlayerColor = color;
        }

        public string Token;
        public string Username;
        public Color PlayerColor;
    }

    public class TurnSequenceDisplay : MonoBehaviour {

        [SerializeField]
        private Text currPlayerText;
        [SerializeField]
        private Transform nextPlayerList;
        [SerializeField]
        private Text nextPlayerPrefab;

        [SerializeField]
        private Color defaultColor;

        private Queue<Text> nextPlayerQueue;   // Queue of next players

        private TurnData[] turnSequence;

        public void SetTurnSequence(TurnData[] turns)
        {
            // Ensure Queue is empty first
            EmptyQueue();

            // Save the TurnData in case of despawn
            turnSequence = (TurnData[])turns.Clone();

            // Enqueue
            foreach (TurnData turn in turns)
            {
                EnqueueTurnPlayer(turn);
            }
        }

        public void GoToNextTurn()
        {
            // Dequeue next player
            if (nextPlayerQueue.Count > 0)
            {
                Text nextPlayer = nextPlayerQueue.Dequeue();

                // Set next player as current
                SetCurrentPlayer(nextPlayer.text, nextPlayer.color);

                // Destroy next player
                Destroy(nextPlayer.gameObject);
            }
            else
            {
                Log.Instance.AppendLine("No more player turns left in this cycle");
                SetCurrentPlayerAsEmpty();
            }
        }

        private void SetCurrentPlayer(string username, Color color)
        {
            currPlayerText.text = username;
            currPlayerText.color = color;
        }

        private void EnqueueTurnPlayer(TurnData data)
        {
            // Instantiate next player Text
            Text nextPlayerText = Instantiate(nextPlayerPrefab, nextPlayerList);
            //nextPlayerText.transform.SetParent(nextPlayerList);

            // Set next player Text
            nextPlayerText.text = data.Username;
            nextPlayerText.color = data.PlayerColor;

            // Enqueue next player Text
            nextPlayerQueue.Enqueue(nextPlayerText);
        }

        public void EmptyQueue()
        {
            if (nextPlayerQueue == null)
                nextPlayerQueue = new Queue<Text>();
            else
            {
                // Dequeue queue
                while (nextPlayerQueue.Count > 0)
                {
                    Text nextPlayer = nextPlayerQueue.Dequeue();
                    Destroy(nextPlayer.gameObject);
                }
            }

            // Set default
            SetCurrentPlayerAsEmpty();
        }

        private void SetCurrentPlayerAsEmpty()
        {
            SetCurrentPlayer("-", defaultColor);
        }

        public void RemovePlayerFromQueue(string removedToken)
        {
            EmptyQueue();

            // Make a shallow copy
            TurnData[] tempTS = (TurnData[])turnSequence.Clone();
            turnSequence = new TurnData[tempTS.Length - 1];

            // Dequeue then add back to Queue
            int idx = 0;
            foreach (TurnData td in tempTS)
            {
                if (td.Token != removedToken)   // not removed
                {
                    // Add back to Queue
                    EnqueueTurnPlayer(td);
                    // Save TurnData
                    turnSequence[idx++] = td;
                }
            }
        }
    }

}