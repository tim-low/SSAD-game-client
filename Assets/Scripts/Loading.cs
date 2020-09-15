using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SuperSad.Networking;
using SuperSad.Networking.Events;

public class Loading : EventListener
{
    [SerializeField]
    private string notLoggedInScene;
    [SerializeField]
    private string loggedInScene;

    [SerializeField]
    private GameObject pleaseWaitText;

    private int loginAttempts = 0;
    // Use this for initialization
    void Start () {
        //PlayerPrefs.SetString("password", null);
        //PlayerPrefs.SetString("username", null);
        //PlayerPrefs.DeleteAll();
        pleaseWaitText.SetActive(false);

        StartCoroutine(LateStart(2f));

    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        Authenticate();
    }
    public void Authenticate()
    {
        if(PlayerPrefs.HasKey("password") && PlayerPrefs.HasKey("username"))
        {
            string password = PlayerPrefs.GetString("password", "");
            string username = PlayerPrefs.GetString("username", "");
            UserState.Instance().username = username;
            loginAttempts++;
            ConstructLoginPacket(username,password);
        }
        else
        {
            Debug.Log("No Token or username in playerprefs");
            SceneManager.LoadScene(notLoggedInScene);
        }
    }

    public void HandleErrorPacket(Packet packet)
    {
        Debug.Log("Error Packet Received");
        ErrorAck ack = new ErrorAck(packet);
        Debug.Log("login " + ack.Message);
        Debug.Log(ack.Message.CompareTo("User is currently logged in. Please try again later."));
        if (ack.Message.CompareTo( "User is currently logged in. Please try again later.") == 0 && loginAttempts <=6)
        {
            pleaseWaitText.SetActive(true);
            StartCoroutine(LateStart(5f));
        }
        else
        {
            SceneManager.LoadScene(notLoggedInScene);
        }
        
    }

    public void HandleLogin(Packet packet)
    {
        Debug.Log("Login Packet Received");
        UserAuthAck ack = new UserAuthAck(packet);
        UserState.Instance().Token = ack.Message;
        Debug.Log("Login Packet Token: " + ack.Message);
        //NetworkStreamManager.Instance.SendLoadCharPacket(ack.Message);
        SceneManager.LoadScene(loggedInScene);
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        // for better separation of concerns (Single Responsibility Principle), these handling of packets should be in separate scripts instead of all-in-one manager like this script
        notifier.Register(HandleErrorPacket, Packets.ErrorAck);
        notifier.Register(HandleLogin, Packets.UserAuthAck);
    }

    public void ConstructLoginPacket(string username, string password)
    {;
        Packet ack = new CmdAuthPlayer()
        {
            Username = username,
            Password = password
        }.CreatePacket();

        Debug.Log("Login Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
    }

}
