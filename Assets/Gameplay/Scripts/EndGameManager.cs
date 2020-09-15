using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SuperSad.Networking.Events;
using SuperSad.Networking;
using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class EndGameManager : EventListener {

        [Header("Managers")]
        [SerializeField]
        private GamePlayerManager playerManager;
        [SerializeField]
        private TurnManager turnManager;
        [SerializeField]
        private GameObject gameplayCanvas;
        [SerializeField]
        private GameObject endGameCanvas;

        [Header("Subdisplays")]
        [SerializeField]
        private GameObject rankingsPanel;
        [SerializeField]
        private GameObject accountProgressionPanel;

        [Header("Shared Display")]
        [SerializeField]
        private Text topicText;
        [SerializeField]
        private string topicPrefixString = "Topic: ";
        [SerializeField]
        private Text numQuestionsText;
        [SerializeField]
        private string numQuestionsPrefixString = "No. Questions: ";
        [SerializeField]
        private GameObject lootboxReward;
        [SerializeField]
        private Text lootboxRewardText;
        [SerializeField]
        private string lootboxRewardString = "Received: ";

        [Header("Ranking List")]
        [SerializeField]
        private Transform rankList;
        [SerializeField]
        private RankListItem rankItemPrefab;
        [SerializeField]
        private Button nextPanelButton;

        [Header("User Level/Experience")]
        [SerializeField]
        private Image expBar;
        [SerializeField]
        private Text expGainedText;
        [SerializeField]
        private Text userExpText;
        [SerializeField]
        private Text userLevelText;
        [SerializeField]
        private Text expToNextLevelText;
        [SerializeField]
        private string toNextLevelString = "To Next Level: ";
        [SerializeField]
        private float fillDurationPerLevel = 3f;
        [SerializeField]
        private GameObject levelUpIndication;
        [SerializeField]
        private GameObject leaveGameButton;

        private GameScore userGameScore = null;
        private const int MaxLevel = 10;

        // for animation
        private IEnumerator animationCoroutine = null;

        private void InitSharedUI()
        {
            // Set topic
            WorldSelectManager.WorldType topic = (WorldSelectManager.WorldType)RoomListingDisplayManager.lobbyId;
            topicText.text = topicPrefixString + topic.ToString();

            // Set no. questions
            numQuestionsText.text = numQuestionsPrefixString + turnManager.TotalTurns;
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            // Subscribe to End Game Ack packet event
            notifier.Register(EndGame, Packets.EndGameAck);
        }

        private void EndGame(Packet packet)
        {
            EndGameAck ack = new EndGameAck(packet);

            // Disable Game UI
            gameplayCanvas.SetActive(false);
            // Enable End Game UI
            endGameCanvas.SetActive(true);
			AudioManager.instance.PlaySFX ("UIPop");
            // Init Shared UI
            InitSharedUI();
            // Start with Ranking Panel
            rankingsPanel.SetActive(true);
            accountProgressionPanel.SetActive(false);

            // Get the user token for this client
            string thisUser = UserState.Instance().Token;

            // check if it is a custom quiz
            bool isCustomQuiz = (RoomListingDisplayManager.lobbyId == (int)WorldSelectManager.WorldType.Custom);

            // Display Player Rankings & Set 
            for (int i = 0; i < ack.GameScores.Length; i++)
            {
                GameScore gs = ack.GameScores[i];

                bool isThisUser = (gs.Token == thisUser);

                // Create RankListItem
                CreateRankListItem(gs, i + 1, isThisUser);

                // Initialize Account Progression details for this client
                if (isThisUser)
                {
                    userGameScore = gs;

                    // if it is not a custom quiz, show account level up
                    if (!isCustomQuiz)
                        InitAccountProgression(gs.CurrentExperience, gs.ExperienceGained, gs.CurrentLevel);

                    // Display Loot Box reward for this client
                    if (gs.NumLootBox > 0)
                    {
                        // Display lootbox reward
                        lootboxReward.SetActive(true);
                        lootboxRewardText.text = lootboxRewardString + gs.NumLootBox + "x";
                    }
                    else
                    {
                        // Disable lootbox
                        lootboxReward.SetActive(false);
                    }
                }

                // if it is custom quiz, set to leave game immediately instead
                if (isCustomQuiz)
                {
                    nextPanelButton.onClick.RemoveAllListeners();
                    nextPanelButton.onClick.AddListener(() => RoomListingDisplayManager.ChangeToRoomScene());
                }
            }
        }

        private void CreateRankListItem(GameScore gs, int rankNo, bool isThisUser)
        {
            RankListItem item = Instantiate(rankItemPrefab, rankList);

            // Get Player
            Player player = playerManager.GetGamePlayer(gs.Token).Player;
            //Color color = globalVariables.GetTileColor(player.Color);
            Color color = Color.white;

            // Set details
            item.SetRank(rankNo, color);
            item.SetUsername(player.Username, color);
            item.SetScore(gs.Score, color);
            item.SetQuestionsCorrect(gs.AnswerCorrectly, color);
            item.SetIsLocalClient(isThisUser);
        }

        /// <summary>
        /// Initialize the interface to display the user's progression
        /// </summary>
        /// <param name="currentExp">The current EXP owned by the user for this level</param>
        /// <param name="expGained">The EXP gained from this game</param>
        /// <param name="currLevel">The user's current level</param>
        private void InitAccountProgression(int currentExp, int expGained, int currLevel)
        {
            int levelExpNeeded = getLevelExpToNextLevel(currLevel);

            // Set Texts
            userExpText.text = currentExp.ToString();
            expGainedText.text = expGained.ToString();
            userLevelText.text = currLevel.ToString();

            if (currLevel == MaxLevel)  // user is at max level
            {
                SetMaxLevelExp();
            }
            else
            {
                expToNextLevelText.text = toNextLevelString + (levelExpNeeded - currentExp).ToString();

                // Set EXP Bar fill
                Vector3 newScale = expBar.transform.localScale;
                newScale.x = (float)currentExp / levelExpNeeded;
                expBar.transform.localScale = newScale;
            }
        }

        private void SetMaxLevelExp()
        {
            expToNextLevelText.text = toNextLevelString + "0";

            // Set EXP Bar fill
            expBar.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        /// <summary>
        /// Next Button OnClick callback
        /// </summary>
        public void ShowAccountProgression()
        {
            // Show Account Progression Panel
            accountProgressionPanel.SetActive(true);
            rankingsPanel.SetActive(false);

            // Disable level up indicator first
            levelUpIndication.SetActive(false);
            //expToNextLevelText.gameObject.SetActive(false);

            // Disable exit/leave button first
            leaveGameButton.SetActive(false);
        }

        /// <summary>
        /// Function callback when player taps on screen
        /// </summary>
        public void AnimateExpBar()
        {
            if (userGameScore.CurrentLevel == MaxLevel) // user is at max level
            {
                // Enable leave game button
                leaveGameButton.SetActive(true);
            }
            else
            {
                // Animate gaining of EXP
                animationCoroutine = AnimateExpGain();
                StartCoroutine(animationCoroutine);
            }
        }

        private IEnumerator AnimateExpGain()
        {
            // Get exp values
            int gainedExpLeft = userGameScore.ExperienceGained;
            int currentExp = userGameScore.CurrentExperience;
            int userLevel = userGameScore.CurrentLevel;

            // Disable exp amount to next level text
            expToNextLevelText.gameObject.SetActive(false);

            while (gainedExpLeft > 0)   // still got EXP gained to fill
            {
                int totalLevelExpNeeded = getLevelExpToNextLevel(userLevel);
                // For checking
                Debug.Log("Gained Exp: " + gainedExpLeft);
                Debug.Log("Current Exp: " + currentExp);
                Debug.Log("Current Level: " + userLevel);
                Debug.Log("Total Exp Needed to next level: " + totalLevelExpNeeded);

                int amountNeededToNextLevel = totalLevelExpNeeded - currentExp;

                int amountToFillThisLevel = Mathf.Min(gainedExpLeft, amountNeededToNextLevel);

                // get duration for this animation before level up (if there is)
                //float durationForThisLevelExp = getFillDurationForThisLevel(gainedExpLeft, amountNeededToNextLevel, fillAnimationDuration);
                // get target scale for animation before level up (if there is)
                int targetExp = currentExp + amountToFillThisLevel; // EXP value after this animation
                float targetScaleX = (float)targetExp / totalLevelExpNeeded;

                Debug.Log("Amount to fill this level: " + amountToFillThisLevel);
                Debug.Log("Target scale: " + targetScaleX);

                // Animate till level up (if there is)
                //yield return FillExpBar(durationForThisLevelExp, targetScaleX);
                yield return FillExpBar(fillDurationPerLevel, targetScaleX, gainedExpLeft, amountToFillThisLevel, currentExp);

                // Update EXP values to check remaining Exp Fill
                currentExp = currentExp + amountToFillThisLevel;
                if (currentExp >= totalLevelExpNeeded)
                    currentExp = 0;
                gainedExpLeft -= amountNeededToNextLevel;

                // Check if there is a level up
                if (gainedExpLeft >= 0) // there is a level up
                {
                    // Update level display
                    userExpText.text = "0";
                    levelUpIndication.SetActive(true);
                    AudioManager.instance.PlaySFX("Levelup");
                    ++userLevel;
                    userLevelText.text = userLevel.ToString();

                    // Check if max level reached
                    if (userLevel == MaxLevel)
                    {
                        break;
                    }

                    // Update EXP bar display
                    expBar.transform.localScale = new Vector3(0f, 1f, 1f);

                    // Play level up SFX
                    // Pause
                    yield return new WaitForSeconds(0.3f);
                }

                // Pause before next display update
                yield return new WaitForSeconds(0.4f);

            }

            // Update to next level
            if (userLevel == MaxLevel)
            {
                SetMaxLevelExp();
            }
            else
            {
                int[] afterExp = getNextLevelExp(userGameScore.ExperienceGained, userGameScore.CurrentLevel, userGameScore.CurrentExperience);
                int levelExpNeeded = getLevelExpToNextLevel(afterExp[0]);
                expToNextLevelText.text = toNextLevelString + (levelExpNeeded - afterExp[1]);
                expToNextLevelText.gameObject.SetActive(true);
            }

            yield return null;

            // Enable leave game button
            leaveGameButton.SetActive(true);
        }

        private IEnumerator FillExpBar(float duration, float targetScaleX, int gainedExpLeft, int amountToFillThisLevel, int currentExp)
        {
            Vector3 scale = expBar.transform.localScale;
            float startScaleX = expBar.transform.localScale.x;
            float timer = 0f;

            // Play SFX
            AudioManager.instance.PlaySFX ("exp1");
            while (timer < duration)
            {
				
                float timeRatio = timer / duration;

                // Update EXP gained value
                float amountFilledSoFar = timeRatio * amountToFillThisLevel;
                float gainedExpRemaining = gainedExpLeft - amountFilledSoFar;
                expGainedText.text = ((int)gainedExpRemaining).ToString();
                // Update owned EXP value
                int newCurrExp = currentExp + (int)amountFilledSoFar;
                userExpText.text = newCurrExp.ToString();

                // Update EXP bar
                scale.x = Mathf.Lerp(startScaleX, targetScaleX, timeRatio);
                expBar.transform.localScale = scale;

                // Update timer
                timer += Time.deltaTime;
                yield return null;
            }

            // Set fill bar to target
            expGainedText.text = (gainedExpLeft - amountToFillThisLevel).ToString();
            userExpText.text = (currentExp + amountToFillThisLevel).ToString();
            expBar.transform.localScale = new Vector3(targetScaleX, scale.y, scale.z);
        }

        private int getLevelExpToNextLevel(int currLevel)
        {
            return currLevel * 1000;
        }

        /// <summary>
        /// Should NOT be here; should be in UserState class
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private int[] getNextLevelExp(int expGained, int currLevel, int currLevelExp)
        {
            int level;
            expGained += currLevelExp;
            for (level = currLevel; level < 10; level++)
            {
                if (expGained < level * 1000)
                {
                    break;
                }
                expGained -= level * 1000;
            }
            int[] result = { level, expGained };
            return result;
        }

        /// <summary>
        /// Function callback for button OnClick
        /// </summary>
        public void LeaveToRoom()
        {
            RoomListingDisplayManager.ChangeToRoomScene();
        }
    }

}