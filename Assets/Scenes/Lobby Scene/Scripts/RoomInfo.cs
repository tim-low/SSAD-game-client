using UnityEngine;
using UnityEngine.UI;
public class RoomInfo : MonoBehaviour
{
    public Text RoomName;
    public Text NumOfPlayers;

    public Image lockIndicator;

    public bool IsLocked = false;

    public bool IsInGame = false;

    public static string[] playerTokens = new string[4];

    public static bool[] readyStates = new bool[4];

    public string RoomID;

    public void SetRoomName(string roomName)
    {
            RoomName.text = roomName;
    }

    public void SetNumOfPlayers(int numOfPlayers)
    {

        NumOfPlayers.text = numOfPlayers.ToString();
    }

    public void SetLocked(bool isLocked)
    {
        IsLocked = isLocked;
        lockIndicator.enabled = true;
    }

    public void SetRoomID(string roomID)
    {
        RoomID = roomID;
    }

    public void SetIsInGame(bool inGame)
    {
        IsInGame = inGame;
    }

}