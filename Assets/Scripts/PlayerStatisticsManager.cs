using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game_Server.Model;
using UnityEngine.SceneManagement;
using SuperSad.Networking;
using SuperSad.Networking.Events;

public class PlayerStatisticsManager : EventListener
{
    [SerializeField]
    private Image[] sliderBaseImages;
    [SerializeField]
    private Image[] sliderImages;
    [SerializeField]
    private Sprite[] starSprites;
    [SerializeField]
    private RectTransform[] starSlider;
    [SerializeField]
    private Text[] starLevelText;
    [SerializeField]
    private Text[] mainPanelExpText;
    [SerializeField]
    private Text subPanelTitle;
    [SerializeField]
    private Text subPanelInfoText;
    [SerializeField]
    private Text gamesPlayedText;
    private PlayerStatsAck playerStats;
    private string[] infoToDisplay = new string[6];
    [SerializeField]
    private string[] titleToDisplay;
    // Use this for initialization
    void Start () {
        ConstructStatsPacket();
    }

    /*IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        ConstructStatsPacket();
    }*/
    // Update is called once per frame
    void Update () {
		
	}
    public void setInfoText(int index)
    {
        subPanelTitle.text = titleToDisplay[index];
        subPanelInfoText.text = infoToDisplay[index];
    }
    public int[] getNextLevelExp(int exp)
    {
        int level;
        for( level = 1; level <= 10; level++)
        {
            if(exp < level*1000)
            {
                break;
            }
            exp -= level * 1000; 
        }
        int[] result = { level, exp };
        return result;
    }


    private void setStar(int level, int topicIndex)
    {
        int star;
        if(level <=4)
        {
            star = 1;
           
            
        }
        else if(level<=7)
        {
            star = 2;
        }
        else
        {
            Debug.Log("gold");
            star = 3;
        }
        if(level>= 10)
        {
            sliderBaseImages[topicIndex].sprite = starSprites[3];
        }
        else
        {
            sliderBaseImages[topicIndex].sprite = starSprites[star - 1];
        }
        
        sliderImages[topicIndex].sprite = starSprites[star];
    }

    public void setUI(int exp , int correctqns, int totalquestions , int topicIndex)
    {
        int[] expList = getNextLevelExp(exp);
        string toExpString = "Exp: " + expList[1] + "/" + (expList[0] * 1000).ToString();
        setStar(expList[0] , topicIndex);
        starSlider[topicIndex].sizeDelta = new Vector2((int)(((float)expList[1] / (expList[0] * 1000)) * 160), 160);
        starLevelText[topicIndex].text = expList[0].ToString();
        mainPanelExpText[topicIndex].text = toExpString;
        infoToDisplay[topicIndex] = (toExpString + "\\n" + "No.of questions\\n" + "answered correctly : " + correctqns + "/" + totalquestions + " ("+((totalquestions == 0)? "0%)":(int)((correctqns / (float)totalquestions) * 100) + "%)")).Replace("\\n","\n");
    }
    public void testUI()
    {
        setUI(2000, 0, 0, 0);
        setUI(1000, 200, 400, 1);
        setUI(10000, 100, 350, 2);
        setUI((50000-6000), 100, 350, 3);
        setUI(20000, 100, 350, 4);
        setUI(2000, 100, 350, 5);
    }

    public void HandleErrorPacket(Packet packet)
    {
        Debug.Log("Error Packet Received");
        ErrorAck ack = new ErrorAck(packet);
        Debug.Log(ack.Message);
        //errorText.text = ack.Message;
        //errorObject.SetActive(true);
    }
    public void HandleStatsPacket(Packet packet)
    {
        Debug.Log("Stats Packet Received");
        playerStats = new PlayerStatsAck(packet);
        Debug.Log(playerStats.RequirementsExp);
        setUI(playerStats.RequirementsExp, playerStats.RequirementsCorrect, playerStats.RequirementsTotalQns , 0);
        setUI(playerStats.DesignExp, playerStats.DeploymentCorrect, playerStats.DesignTotalQns , 1);
        setUI(playerStats.ImplementationExp, playerStats.ImplementationCorrect, playerStats.ImplementationTotalQns , 2);
        setUI(playerStats.TestingExp, playerStats.TestingCorrect, playerStats.TestingTotalQns , 3);
        setUI(playerStats.DeploymentExp, playerStats.DeploymentCorrect, playerStats.DeploymentTotalQns , 4);
        setUI(playerStats.MaintenanceExp, playerStats.MaintenanceCorrect, playerStats.MaintenanceTotalQns ,5);
        gamesPlayedText.text = "Games Played: " + playerStats.GameCount;
    }
    public override void Subscribe(INotifier<Packet> notifier)
    {
        // for better separation of concerns (Single Responsibility Principle), these handling of packets should be in separate scripts instead of all-in-one manager like this script
        notifier.Register(HandleErrorPacket, Packets.ErrorAck);
        notifier.Register(HandleStatsPacket, Packets.PlayerStatsAck);
    }

    public void ConstructStatsPacket()
    {
        Packet ack = new CmdPlayerStats()
        {
            Token = UserState.Instance().Token
        }.CreatePacket();

        Debug.Log("Stats Packet Created");
        if(ack == null)
        {
            Debug.Log("why null");
        }
        else
        {
            Debug.Log("sending packet ooooooooo");
            NetworkStreamManager.Instance.SendPacket(ack);
        }
       
    }
}
