using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserListItem : MonoBehaviour {

    [SerializeField]
    private Text usernameText;

    /*
    public void SetPlayerText(int playerId, Color color)
    {
        playerText.text = "Player " + playerId;
        playerText.color = color;
    }*/

    public void SetUsername(string username, Color color)
    {
        usernameText.text = username;
        usernameText.color = color;
    }
}
