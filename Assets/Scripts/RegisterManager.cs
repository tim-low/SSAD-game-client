using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game_Server.Model;
using UnityEngine.SceneManagement;
using SuperSad.Networking;
using SuperSad.Networking.Events;
using System.Text.RegularExpressions;

public class RegisterManager : EventListener
{
    [SerializeField]
    private InputField usernameText;
    [SerializeField]
    private InputField passwordText;
    [SerializeField]
    private InputField emailText;
    [SerializeField]
    private InputField classText;
    [SerializeField]
    private InputField yearText;
    [SerializeField]
    private InputField studentNameText;
    [SerializeField]
    private Dropdown semesterText;
    [SerializeField]
    private GameObject errorObject;
    [SerializeField]
    private Text errorText;

    [SerializeField]
    private string changeToScene;

    private bool expectError = false;

    [SerializeField]
    private ManageScene manageScene;

    private Regex regexUserName = new Regex("^[a-zA-Z0-9_]*$");
    private Regex regexPassword = new Regex("^(?=.*[A-Z])(?=.*[0-9]).*$");
    private Regex regexEmail = new Regex(@"^([\w\.\-]+)@[\w\.]*ntu.edu\.sg$");
    public void HandleErrorPacket(Packet packet)
    {
        if(expectError == false)
        {
            return;
        }
        expectError = false;
        Debug.Log("Error Packet Received");
        ErrorAck ack = new ErrorAck(packet);
        Debug.Log("Register" + ack.Message);
        errorText.text = ack.Message;
        errorObject.SetActive(true);
    }

    public void HandleRegister(Packet packet)
    {
        Debug.Log("Register Packet Received");
        UserRegAck ack = new UserRegAck(packet);
        UserState.Instance().Token = ack.Token;
        Debug.Log("Register Packet Token: " + ack.Token);
        PlayerPrefs.SetString("password", LoginManager.Sha1Sum2(passwordText.text));
        PlayerPrefs.SetString("username", usernameText.text);
        UserState.Instance().username = usernameText.text;
        manageScene.LoadScene(changeToScene);
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        // for better separation of concerns (Single Responsibility Principle), these handling of packets should be in separate scripts instead of all-in-one manager like this script
        notifier.Register(HandleErrorPacket, Packets.ErrorAck);
        notifier.Register(HandleRegister, Packets.UserRegAck);
    }

    public void ConstructRegisterPacket()
    {
        Debug.Log("class: " + classText.text);
        Debug.Log("year: " + yearText.text);
        if (!validateRegisterInput())
        {
            return;
        }
        expectError = true;
        Packet ack = new CmdUserReg()
        {
            Username = usernameText.text,
            Password = LoginManager.Sha1Sum2(passwordText.text),
            Email = emailText.text,
            StudentName = studentNameText.text,
            Class = classText.text,
            Semester = semesterText.value + 1,
            Year = int.Parse(yearText.text)
        }.CreatePacket();

        Debug.Log("Register Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
    }

    private bool validateRegisterInput()
    {
        bool valid = true;
        
        if(usernameText.text.Length < 6 || usernameText.text.Length > 13)
        {
            errorText.text = "Username length must be at least 6 characters and at most 13 characters. Please enter a different username.";
            valid = false;
        }
        else if(!regexUserName.IsMatch(usernameText.text))
        {
            errorText.text = "Username entered must consists of only alphabets, underscore and numbers. Please enter a different username.";
            valid = false;
        }
        else if (passwordText.text.Length < 8 || passwordText.text.Length > 24)
        {
            errorText.text = "Password length must be at least 8 characters and at most 24 characters. Please enter a different username.";
            valid = false;
        }
        else if (!regexPassword.IsMatch(passwordText.text))
        {
            errorText.text = "Password must contain at least one uppercase and one number. Please enter a different password.";
            valid = false;
        }
        else if(!regexEmail.IsMatch(emailText.text))
        {
            errorText.text = "Email address is not a valid ntu email address. Please enter a different email address.";
            valid = false;
        }
        else if (classText.text.Length > 4)
        {
            errorText.text = "Class length must be at most 4 characters. Please enter a different class.";
            valid = false;
        }
        if (valid == false)
        {
            errorObject.SetActive(true);
        }
        return valid;
    }

}
