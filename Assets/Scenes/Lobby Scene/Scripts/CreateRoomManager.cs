﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SuperSad.Networking;
using SuperSad.Networking.Events;

class CreateRoomManager : EventListener
{
    [SerializeField]
    private InputField roomName;

    [SerializeField]
    private Toggle havePW;

    [SerializeField]
    private InputField password;

    [SerializeField]
    private Dropdown numOfTurns;

    [SerializeField]
    private GameObject ErrorMessage;

    public void HandleCreateRoom(Packet packet)
    {
        Debug.Log("CreateRoomAck Packet Received");
        CreateRoomAck ack = new CreateRoomAck(packet);
        RoomDetailsManager.roomID = ack.Room.RoomId;
        //SceneManager.LoadScene("RoomScene");
        RoomListingDisplayManager.ChangeToRoomScene();
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(HandleCreateRoom, Packets.CreateRoomAck);
    }

    public void ConstructCreateRoomPacket()
    {
        Packet cmd;
        Debug.Log(havePW);
        if (havePW.isOn)
        {
            if(password.text == "" ) {
                ErrorMessage.SetActive(true);
                GameObject  ErrorText = ErrorMessage.transform.GetChild (0).gameObject;
                ErrorText.GetComponentInChildren<Text>().text = "Key In Password";
                return;
            }
            if(roomName.text == "") {
                ErrorMessage.SetActive(true);
                GameObject  ErrorText = ErrorMessage.transform.GetChild (0).gameObject;
                ErrorText.GetComponentInChildren<Text>().text = "Key In Room Name";
                return;
            }
            cmd = new CmdCreateRoom()
            {
                IsLocked = true,
                Token = UserState.Instance().Token,
                RoomName = roomName.text,
                NumTurns = int.Parse(numOfTurns.options[numOfTurns.value].text),
                Password = Sha1Sum2(password.text),

            }.CreatePacket();
        }
        else
        {
             if(roomName.text == "") {
                ErrorMessage.SetActive(true);
                GameObject  ErrorText = ErrorMessage.transform.GetChild (0).gameObject;
                ErrorText.GetComponentInChildren<Text>().text = "Key In Room Name";
                return;
            }
            cmd = new CmdCreateRoom()
            {
                IsLocked = false,
                Token = UserState.Instance().Token,
                RoomName = roomName.text,
                NumTurns = int.Parse(numOfTurns.options[numOfTurns.value].text),

            }.CreatePacket();
        }

        Debug.Log("CmdCreateRoom Packet Created");

        NetworkStreamManager.Instance.SendPacket(cmd);
    }

    public static string Sha1Sum2(string str)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        byte[] bytes = encoding.GetBytes(str);
        var sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        return System.BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", string.Empty).ToLower();
    }
}
