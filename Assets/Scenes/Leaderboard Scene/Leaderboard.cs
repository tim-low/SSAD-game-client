using UnityEngine;
using System.IO;
using UnityEngine.UI;
using SuperSad.Networking.Events;
using SuperSad.Networking;
using System;

namespace Leaderboards
{
    public class Leaderboard : EventListener
    {
        //max entries per screen
        [SerializeField] private int entriesPerPage = 10;
        //parent that holds the highscore objects
        [SerializeField] private Transform UIHolderTransform;

        //highscore object
        [SerializeField] private GameObject leaderboardsEntryObject;
        [SerializeField] private GameObject hallOfFameEntryObject;


        private int pageNumber = 0;

        public Text titleText;

        public Text rankText;   //just to disable it
        public Text scoreOrDateText;

        public Image leftButtonImage;       //to deactivate image
        public Image rightButtonImage;

        string playerName;
        int playerRank;

        


        [Header("Test")]
        [SerializeField] LeaderboardEntryData testEntryData;
        [SerializeField] private Text playerNameAndRank;


        //error display
        [SerializeField]
        private GameObject errorObject;
        public Text errorText;
        private bool expectError = false;

        //network setup
        private LeaderboardEntryData[] leaderboardEntries;
        private HallOfFameEntryData[] hallOfFameEntries;
        public bool isLastPage;
        public int lifeCycleStage;
        public bool hallOfFame = false;

        /*  ENUM:  
            Requirements = 0
            Design = 1
            Implementaiton = 2
            Testing = 3
            Deployment = 4
            Maintenance = 5
            TotalSum = 6
        */

        //LifeCycleStage Setup
        public Button NextStageButton;
        public Button PreviousStageButton;

        //PagesButton Setup
        public Button NextPageButton;
        public Button PreviousPageButton;
        public Text NextPageText;
        public Text PreviousPageText;

        //Hall of Fame setup
        public Button HallOfFameButton;
        public Button LeaderboardsButton;


        // Use this for initialization
        void Start()
        {
            lifeCycleStage = (int)LifeCycleStagesEnum.TotalSum;
            ConstructLeaderboardPacket();
        }

        private void UpdateLeaderboardUI(LeaderboardEntryData[] entries)
        {
            //Set Heading to Leaderboard
            switch (lifeCycleStage)
            {
                //case 0:
                //    titleText.text = "Leaderboard";
                //    break;
                //case 1:
                //    titleText.text = "Requirements";
                //    break;
                //case 2:
                //    titleText.text = "Design";
                //    break;
                //case 3:
                //    titleText.text = "Implementation";
                //    break;
                //case 4:
                //    titleText.text = "Testing";
                //    break;
                //case 5:
                //    titleText.text = "Deployment";
                //    break;
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    titleText.text = lifeCycleStage.ToString();
                    titleText.text = ((LifeCycleStagesEnum)lifeCycleStage).ToString();
                    break;
                default:
                    titleText.text = "Leaderboards";
                    break;
            }


            //Display Player Rank
            playerNameAndRank.text = playerName + ": Rank " + playerRank;
            rankText.text = "Rank";
            scoreOrDateText.text = "Score";

            //Enable Life Cycle Stage buttons
            NextStageButton.gameObject.SetActive(true);
            PreviousStageButton.gameObject.SetActive(true);

            //Set up dummy entry for Testing
            if ((playerName == null) | (playerRank < 1))
            {
                playerName = "playerName";
                playerRank = 0;
            }

            //For each object in the parent gameObject, destroy each one
            foreach (Transform child in UIHolderTransform)
            {
                Destroy(child.gameObject);
            }
            
            //For each entry, populate the gameObject
            for (int i=1; i<= entriesPerPage; i++)
            {
                if (i > entries.Length)
                {
                    Debug.Log("End of entries");    //bug: 2x entries?
                    break;
                }

                Debug.Log(entries[i-1].ToString() + " Received"); //bug: 2x entries?

                Instantiate(leaderboardsEntryObject, UIHolderTransform)
                    .GetComponent<LeaderboardEntryUI>().Initialise(entries[i - 1], pageNumber * 10 + i);       //entries[i]
            }
            Debug.Log(pageNumber.ToString());   //bug: 2x entries?


            //Enable or Disable Next Page/Previous Page Buttons
            if (pageNumber == 0)
            {
                PreviousPageButton.gameObject.SetActive(false);
                PreviousPageText.gameObject.SetActive(false);
            }
            else if (!PreviousPageButton.gameObject.activeSelf)
            {
                PreviousPageButton.gameObject.SetActive(true);
                PreviousPageText.gameObject.SetActive(true);
            }

            if (isLastPage)
            {
                NextPageButton.gameObject.SetActive(false);
                NextPageText.gameObject.SetActive(false);
            }
            else if (!NextPageButton.gameObject.activeSelf)
            {
                NextPageButton.gameObject.SetActive(true);
                NextPageText.gameObject.SetActive(true);
            }
        }


        private void UpdateHallOfFameUI(HallOfFameEntryData[] entries)
        {

            titleText.text = "Hall Of Fame";
            playerNameAndRank.text = "";

            rankText.text = "";
            scoreOrDateText.text = "Date";

            //For each object in the parent gameObject, destroy each one
            foreach (Transform child in UIHolderTransform)
            {
                Destroy(child.gameObject);
            }

            //For each entry, populate the gameObject
            for (int i = 0; i < entriesPerPage; i++)
            {
                if (i >= entries.Length)
                    break;

                Instantiate(hallOfFameEntryObject, UIHolderTransform)
                    .GetComponent<HallOfFameEntryUI>().Initialise(entries[i]);       //entries[i]
            }

            //Disable Life Cycle Stage Selection Buttons
            if ((NextStageButton.gameObject.activeSelf) | (PreviousStageButton.gameObject.activeSelf))
            {
                NextStageButton.gameObject.SetActive(false);
                PreviousStageButton.gameObject.SetActive(false);
            }

            //Enable or Disable Next Page/Previous Page Buttons
            if (pageNumber == 0)
            {
                PreviousPageButton.gameObject.SetActive(false);
            }
            else if (!PreviousPageButton.gameObject.activeSelf)
            {
                PreviousPageButton.gameObject.SetActive(true);
            }
                

            if (isLastPage)
            {
                NextPageButton.gameObject.SetActive(false);
            }
            else if (!NextPageButton.gameObject.activeSelf)
            {
                NextPageButton.gameObject.SetActive(true);
            }
                
        }
        //    //Enable or Disable Next Page/Previous Page Buttons
        //    if (pageNumber == 0)
        //    {
        //        if (leftButtonImage.enabled)
        //            leftButtonImage.enabled = false;
        //    }
        //    else if (!leftButtonImage.enabled)
        //        leftButtonImage.enabled = true;

        //    if (isLastPage)
        //    {
        //        if (rightButtonImage.enabled)
        //            rightButtonImage.enabled = false;
        //    }
        //    else if (!rightButtonImage.enabled)
        //        rightButtonImage.enabled = true;
        //}


        //get saved data. Not needed for the current project
        /*
        private LeaderboardSaveData GetSavedScores()
        {
            //if the file isn't created yet
            if (!File.Exists(SavePath))
            {
                File.Create(SavePath).Dispose();    //create file. dispose of usage afterwards so file isn't always "in use"
                return new LeaderboardSaveData();   //empty list
            }
            else
            {
                //open stream when writing, close stream when finished. Prevents file leakages or forgetting to close
                using (StreamReader stream = new StreamReader(SavePath))
                {

                    //get a stream of data from savepath    
                    string json = stream.ReadToEnd();       

                    //convert jason to a LeaderboardSaveData object
                    return JsonUtility.FromJson<LeaderboardSaveData>(json);
                }
            }
        }
        */

        //save scores to local. not needed for the current project.
        /*
        private void SaveScores(LeaderboardSaveData leaderboardSaveData)
        {   
            using (StreamWriter stream = new StreamWriter(SavePath))
            {   
                //convert leaderboardSaveData to a string
                string json = JsonUtility.ToJson(leaderboardSaveData, true);

                //write a stream of data to the savepath
                stream.Write(json);
            }
        }
        */

        public void onLeftButton()
        {
            if (pageNumber > 0)
            {
                print("left button pressed");
                pageNumber--;

                if (hallOfFame)
                {
                    ConstructHallOfFamePacket();
                }
                else
                {
                    ConstructLeaderboardPacket();
                }
            }
        }
        

        public void OnRightButton()
        {

            //if ((pageNumber < (leaderboardEntries.Length-1)/10) && (pageNumber < (entriesPerPage - 1) / 10))
            if(!isLastPage)
            {
                print("right button pressed");
                pageNumber++;
                print(pageNumber);

                if (hallOfFame)
                {
                    ConstructHallOfFamePacket();
                }
                else
                {
                    ConstructLeaderboardPacket(); 
                }
            }
        }

        public void OnNextStageButton()
        {
            lifeCycleStage++;
            lifeCycleStage %= 7;
            pageNumber = 0;
            ConstructLeaderboardPacket();
        }

        public void OnPreviousStageButton()
        {
            lifeCycleStage--;
            lifeCycleStage %= 7;
            lifeCycleStage = (lifeCycleStage < 0) ? lifeCycleStage + 7 : lifeCycleStage;
            pageNumber = 0;

            ConstructLeaderboardPacket();
        }

        public void OnHallOfFameButton()
        {
            hallOfFame = true;
            lifeCycleStage = 6;
            pageNumber = 0;
            ConstructHallOfFamePacket();


            HallOfFameButton.gameObject.SetActive(false);
            LeaderboardsButton.gameObject.SetActive(true);
        }

        public void OnLeaderboardsButton()
        {
            hallOfFame = false;
            lifeCycleStage = 6;
            pageNumber = 0;
            ConstructLeaderboardPacket();

            HallOfFameButton.gameObject.SetActive(true);
            LeaderboardsButton.gameObject.SetActive(false);
        }

        public void ConstructHallOfFamePacket()
        {
            expectError = true;
            //Debug.Log(Crypto.ComputeSHA1Hash(passwordText.text));
            //Debug.Log(Sha1Sum2(passwordText.text));
            Packet ack = new CmdHallOfFame()
            {
                PageNumber = pageNumber,
                NumEntries = entriesPerPage,
                
            }.CreatePacket();

            Debug.Log("HallOfFame Packet Created");

            NetworkStreamManager.Instance.SendPacket(ack);
        }


        public void ConstructLeaderboardPacket()
        {
            expectError = true;
            //Debug.Log(Crypto.ComputeSHA1Hash(passwordText.text));
            //Debug.Log(Sha1Sum2(passwordText.text));
            Packet ack = new CmdLeaderboard()
            {
                Token = UserState.Instance().Token,
                PageNumber = pageNumber,
                NumEntries = entriesPerPage,
                
                LifeCycleStage = lifeCycleStage
            }.CreatePacket();

            Debug.Log("Leaderboard Packet Created");

            NetworkStreamManager.Instance.SendPacket(ack);
        }

        public override void Subscribe(INotifier<Packet> notifier)
        {
            notifier.Register(HandleErrorPacket, Packets.ErrorAck);
            notifier.Register(HandleLeaderboardDisplay, Packets.LeaderboardAck);

            notifier.Register(HandleHallOfFameDisplay, Packets.HallOfFameAck);
        }

        private void HandleHallOfFameDisplay(Packet packet)
        {
            HallOfFameAck ack = new HallOfFameAck(packet);                                //TODO: Token is probably wrong
            Debug.Log("HallOfFame Packet Received");
            Debug.Log("HallOfFame Packet Token: " + ack.ToString());

            isLastPage = ack.IsLastPage;
            hallOfFameEntries = ack.Entries;
            Debug.Log(hallOfFameEntries);
            UpdateHallOfFameUI(hallOfFameEntries);
        }

        private void HandleLeaderboardDisplay(Packet packet)
        {
            LeaderboardAck ack = new LeaderboardAck(packet);                                //TODO: Token is probably wrong
            Debug.Log("Leaderboard Packet Received");
            Debug.Log("Leaderboard Packet Token: " + ack.ToString());

            playerName = UserState.Instance().username;                                                //TODO: UserState.username

            playerRank = ack.OwnRank;
            isLastPage = ack.IsLastPage;
            leaderboardEntries = ack.Entries;
            Debug.Log(leaderboardEntries);
            UpdateLeaderboardUI(leaderboardEntries);
        }

        private void HandleErrorPacket(Packet packet)
        {
            if (expectError == false)
            {
                return;
            }
            expectError = false;
            Debug.Log("Error Packet Received");
            ErrorAck ack = new ErrorAck(packet);
            Debug.Log("login " + ack.Message);
            errorText.text = ack.Message;
            errorObject.SetActive(true);
        }
    }
}
