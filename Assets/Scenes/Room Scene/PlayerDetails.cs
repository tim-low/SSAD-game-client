using UnityEngine;
using UnityEngine.UI;
public class PlayerDetails : MonoBehaviour
{
    public Text PlayerName;
    public Text ReadyStatus;

    public Image hostIndicator;


    public string Token;

    public void SetPlayerName(string playerName)
    {
        PlayerName.text = playerName;
    }

    public void SetStatus(string readyStatus)
    {

        ReadyStatus.text = readyStatus;
    }

    public void SetPlayerToken(string token) {
        Token = token;
    }

    public void SetHost() {
        hostIndicator.enabled = !hostIndicator.enabled;
    }
}