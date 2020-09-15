using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game_Server.Model;
using UnityEngine.SceneManagement;
using SuperSad.Networking;
using SuperSad.Networking.Events;

public class LoginManager : EventListener{
    [SerializeField]
    private InputField usernameText;
    [SerializeField]
    private InputField passwordText;
    [SerializeField]
    private GameObject errorObject;
    public Text errorText;

    [SerializeField]
    private string changeToScene;

    private bool expectError = false;

    [SerializeField]
    private ManageScene manageScene;

    public void HandleErrorPacket(Packet packet)
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

    public void HandleLogin(Packet packet)
    {
        Debug.Log("Login Packet Received");
        UserAuthAck ack = new UserAuthAck(packet);
        UserState.Instance().Token = ack.Message;
        Debug.Log("Login Packet Token: " + ack.Message);
        UserState.Instance().username = usernameText.text;
        PlayerPrefs.SetString("password", Sha1Sum2(passwordText.text));
        PlayerPrefs.SetString("username", usernameText.text);
        manageScene.LoadScene(changeToScene);
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        // for better separation of concerns (Single Responsibility Principle), these handling of packets should be in separate scripts instead of all-in-one manager like this script
        notifier.Register(HandleErrorPacket, Packets.ErrorAck);
        notifier.Register(HandleLogin, Packets.UserAuthAck);
    }

    public void ConstructLoginPacket()
    {
        if (!validateLoginInput())
        {
            return;
        }
        expectError = true;
        //Debug.Log(Crypto.ComputeSHA1Hash(passwordText.text));
        Debug.Log(Sha1Sum2(passwordText.text));
        Packet ack = new CmdAuthPlayer()
        {
            Username = usernameText.text,
            Password = Sha1Sum2(passwordText.text)
        }.CreatePacket();

        Debug.Log("Login Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
    }

    public static string Sha1Sum2(string str)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        byte[] bytes = encoding.GetBytes(str);
        var sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        return System.BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", string.Empty).ToLower();
    }
    private bool validateLoginInput()
    {
        bool valid = true;

        if (usernameText.text.Length > 13)
        {
            errorText.text = "Username length cannot be more than 13 characters. Please enter a different username.";
            valid = false;
        }
        if (valid == false)
        {
            errorObject.SetActive(true);
        }
        return valid;
    }
}
