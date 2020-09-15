using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Networking.Events;
using SuperSad.Networking;
using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class QuestionManager : EventListener
    {
        [SerializeField]
        private ItemEffectManager itemEffectManager;
        [SerializeField]
        private GameObject questionPopup;

        [Header("Question UI Elements")]
        [SerializeField]
        private GameObject questionUI;
        [SerializeField]
        private AnswerButton[] answerButtons;
        [SerializeField]
        private Text questionText;
        [SerializeField]
        private CountdownTimer countdownTimer;

        [Header("Results UI Elements")]
        [SerializeField]
        private GameObject resultsUI;
        [SerializeField]
        private Text resultText;
        [SerializeField]
        private Text stepsRewardText;
        [SerializeField]
        private Text itemRewardText;
        [SerializeField]
        private Image itemRewardImage;
        [SerializeField]
        private Text itemDescriptionText;

        [Header("Results Text")]
        [SerializeField]
        private string correctAnswerString = "Congratulations! That's correct!";
        [SerializeField]
        private string wrongAnswerString = "Oops! That's wrong!";
        [SerializeField]
        private string timeoutAnswerString = "You ran out of time!";
        [SerializeField]
        private string stepsRewardString = "Steps: ";
        [SerializeField]
        private string itemRewardString = "Item: ";

        public void SelectAnswer(AnswerButton ansButton)
        {
            // Construct Packet to send
            Packet cmd = new CmdSelectAnswer()
            {
                Token = UserState.Instance().Token,
                SelectedAnswer = ansButton.AnswerId
            }.CreatePacket();

            // Send Packet
            NetworkStreamManager.Instance.SendPacket(cmd);

            // Disable AnswerButtons
            DisableAnswerButtons();

            // Set answer button as selected
            ansButton.SetSelectedColor();
        }

        public void DisableAnswerButtons()
        {
            foreach (AnswerButton button in answerButtons)
            {
                // Disable button interactivity
                button.GetComponent<Button>().interactable = false;
                // Reset button colour
                button.SetDefaultColor();
            }
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to start quiz packet event
            notifier.Register(StartQuizAck, Packets.StartQuizAck);

            // Subscribe to select answer ack packet event
            notifier.Register(DisplayAnswerResults, Packets.SelectAnswerAck);

            // Subscribe to initialize cycle ack packet event
            notifier.Register(HideQuestionPopup, Packets.InitializeCycleAck);
        }

        private void DisplayUI(bool isQuestion)
        {
            questionUI.SetActive(isQuestion);
            resultsUI.SetActive(!isQuestion);
			AudioManager.instance.PlaySFX ("UIPop");
        }

        private void HideQuestionPopup(Packet packet)
        {
            questionPopup.gameObject.SetActive(false);
        }

        private void StartQuizAck(Packet packet)
        {
            StartQuizAck ack = new StartQuizAck(packet);

            // Enable Question Popup
            questionPopup.gameObject.SetActive(true);
            // Display Question
            DisplayUI(true);

            // Set Question Text
            questionText.text = ack.Question;
            // Set Answers
            for (int i = 0; i < ack.Answers.Length; i++)
            {
                // Enable Answer button
                answerButtons[i].GetComponent<Button>().interactable = true;
                // Set Buttons back to default
                answerButtons[i].SetDefaultColor();
                // Set Answer info
                answerButtons[i].SetAnswer(ack.Answers[i]);
            }

            // Set countdown timer
            countdownTimer.StartTimer(ack.Duration, true);
        }

        private void DisplayAnswerResults(Packet packet)
        {
            SelectAnswerAck ack = new SelectAnswerAck(packet);

            // Display Results
            DisplayUI(false);

            // Set results
            foreach (QuizReward reward in ack.Rewards)
            {
                if (reward.Token == UserState.Instance().Token)
                {
                    UpdateRewardUI(reward);
                    break;
                }
            }
        }

        private void UpdateRewardUI(QuizReward reward)
        {
            // Set result
            resultText.text = reward.IsTimeout ? timeoutAnswerString : (reward.IsCorrect ? correctAnswerString : wrongAnswerString);
            Debug.Log("Answer Correct: " + reward.IsCorrect);

            // Set Rewarded Steps
            stepsRewardText.text = stepsRewardString + reward.StepsAwarded;

			//Play Sfx
			if (reward.IsCorrect) {
				AudioManager.instance.PlaySFX ("Correct");
			}

            // Set Reward Item
            if (reward.ItemAwarded == ItemType.NIL)    // no reward
            {
                // Set Item Sprite
                itemRewardImage.gameObject.SetActive(false);
                // Set Item Name
                itemRewardText.text = itemRewardString + "-";
                // Set Item Description
                itemDescriptionText.text = "";
            }
            else
            {
                // Set Item Name
                string itemName = reward.ItemAwarded.ToString();
                itemRewardText.text = itemRewardString + itemName;

                // Set Item Sprite
                Sprite sprite = Resources.Load<Sprite>("ItemSprites/" + itemName);
                if (sprite != null)
                {
                    itemRewardImage.sprite = sprite;
                    itemRewardImage.gameObject.SetActive(true);
                }
                else
                    itemRewardImage.gameObject.SetActive(false);

                // Set Item Description
                itemDescriptionText.text = itemEffectManager.GetItemDescription(reward.ItemAwarded);
            }
				
        }
    }

}