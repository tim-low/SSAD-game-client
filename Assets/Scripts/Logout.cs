using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperSad.Networking;

public class Logout : MonoBehaviour {
    [SerializeField]
    private ManageScene manageScene;
    [SerializeField]
    private string notLoggedInScene;
    public void LogOut()
    {
        PlayerPrefs.DeleteKey("password");
        PlayerPrefs.DeleteKey("username");
        Packet ack = new CmdUserLogout()
        {
            
        }.CreatePacket();

        Debug.Log("Logout Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
        manageScene.LoadScene(notLoggedInScene);
    }
}
