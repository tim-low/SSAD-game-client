﻿using UnityEngine;
using SuperSad.Model;
using SuperSad.Networking;
using SuperSad.Networking.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RoomListingDisplayManager : EventListener
{


    [SerializeField]
    private GameObject RoomListingPlayingPrefab;

    [SerializeField]
    private GameObject RoomListingWaitingPrefab;

    [SerializeField]

    private GameObject AllList;

    [SerializeField]

    private GameObject WaitingList;

    [SerializeField]
    private GameObject PlayingList;

    [SerializeField]
    private GridLayoutGroup PlayingGrid;

    [SerializeField]
    private GridLayoutGroup WaitingGrid;
    [SerializeField]
    private GridLayoutGroup AllGrid;
    [SerializeField]
    private GameObject lobbyScene;

    [SerializeField]
    private GameObject createRoomScene;

    [SerializeField]
    private GameObject lockedRoom;

    [SerializeField]

    public InputField enterPassword;

    [SerializeField]
    public GameObject errorMsg;

    [SerializeField]
    public Text RoomNameText;

    [SerializeField]
    private Button AllButton;
    [SerializeField]
    private Button WaitingButton;
    [SerializeField]
    private Button PlayingButton;
    [SerializeField]
    private GameObject BG;
    [SerializeField]
    private GameObject HorizontalGroup;

    [SerializeField]
    public List<GameObject> AllRoomList = new List<GameObject>();

    [SerializeField]
    public List<GameObject> WaitingRoomList = new List<GameObject>();

    [SerializeField]
    public List<GameObject> PlayingRoomList = new List<GameObject>();

    private List<Room> roomList = new List<Room>();

    public static int lobbyId = 0;

    public bool changeStatus = false;

    public bool exitCreateRoom = false;


    void OnEnable()
    {
        AllButton.interactable = false;
        WaitingList.SetActive(true);
        PlayingList.SetActive(true);
        RoomListingDisplay();
    }
    private void RoomListingDisplay()
    {
        // Clear Rooms List first
        ClearAllRooms();
        ClearPlayingRooms();
        ClearWaitingRooms();

        lobbyScene.SetActive(true);
        AllList.SetActive(true);
        WaitingList.SetActive(false);
        PlayingList.SetActive(false);
        if (exitCreateRoom)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                Room room = (Room)roomList[i];
                if (room.IsInGame)
                {
                    AddToAllRoomList(room.RoomId, room.RoomName, room.IsLocked, room.NoOfUser, true);
                    AddToPlayingRoomList(room.RoomId, room.RoomName, room.IsLocked, room.NoOfUser);
                }
                else
                {
                    AddToAllRoomList(room.RoomId, room.RoomName, room.IsLocked, room.NoOfUser, false);
                    AddToWaitingRoomList(room.RoomId, room.RoomName, room.IsLocked, room.NoOfUser);
                }
            }
            exitCreateRoom = false;
        }
        else
        {
            roomList = new List<Room>(WorldSelectManager.roomList);
            for (int i = 0; i < roomList.Count; i++)
            {
                Room room = (Room)roomList[i];
                if (room.IsInGame)
                {
                    AddToAllRoomList(room.RoomId, room.RoomName, room.IsLocked, room.NoOfUser, true);
                    AddToPlayingRoomList(room.RoomId, room.RoomName, room.IsLocked, room.NoOfUser);
                }
                else
                {
                    AddToAllRoomList(room.RoomId, room.RoomName, room.IsLocked, room.NoOfUser, false);
                    AddToWaitingRoomList(room.RoomId, room.RoomName, room.IsLocked, room.NoOfUser);
                }
            }
        }
    }

    private void ClearAllRooms()
    {
        for (int i = 0; i < AllRoomList.Count; i++)
        {
            GameObject room = AllRoomList[i];
            Destroy(room);
            AllRoomList.Remove(room);
        }
    }

    private void ClearWaitingRooms()
    {
        for (int i = 0; i < WaitingRoomList.Count; i++)
        {
            GameObject room = WaitingRoomList[i];
            Destroy(room);
            WaitingRoomList.Remove(room);
        }
    }

    private void ClearPlayingRooms()
    {
        for (int i = 0; i < PlayingRoomList.Count; i++)
        {
            GameObject room = PlayingRoomList[i];
            Destroy(room);
            PlayingRoomList.Remove(room);
        }
    }


    public void OnClickPlaying()
    {
        AllList.SetActive(false);
        WaitingList.SetActive(false);
        PlayingList.SetActive(true);
        PlayingButton.interactable = false;
        AllButton.interactable = true;
        WaitingButton.interactable = true;

    }

    public void OnClickAll()
    {
        AllList.SetActive(true);
        WaitingList.SetActive(false);
        PlayingList.SetActive(false);
        PlayingButton.interactable = true;
        AllButton.interactable = false;
        WaitingButton.interactable = true;
    }

    public void OnClickWaiting()
    {
        AllList.SetActive(false);
        WaitingList.SetActive(true);
        PlayingList.SetActive(false);
        PlayingButton.interactable = true;
        AllButton.interactable = true;
        WaitingButton.interactable = false;
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(HandleRoomCreated, Packets.RoomCreatedAck);
        notifier.Register(HandleUpdateRoomPlayerCount, Packets.UpdateRoomPlayerCountAck);
        notifier.Register(HandleUpdateRoomStatus, Packets.UpdateRoomStatusAck);
    }

    public void HandleUpdateRoomStatus(Packet packet)
    {
        Debug.Log("UpdateRoomStatusAck Packet Received");
        UpdateRoomStatusAck ack = new UpdateRoomStatusAck(packet);
        if (ack.IsInGame)
        {
            changeStatus = true;
            RemoveFromAllRoomList(ack.RoomId);
            RemoveFromWaitingRoomList(ack.RoomId);
            changeStatus = false;
        }
        else
        {
            changeStatus = true;
            RemoveFromAllRoomList(ack.RoomId);
            RemoveFromPlayingRoomList(ack.RoomId);
            changeStatus = false;
        }
    }

    public void OnClickCreateRoom()
    {
        createRoomScene.SetActive(true);
        AllList.SetActive(false);
        WaitingList.SetActive(false);
        PlayingList.SetActive(false);
        HorizontalGroup.SetActive(false);
        BG.SetActive(false);
    }

    public void AddToAllRoomList(string roomId, string roomName, bool isLocked, int numOfPlayers, bool inGame)
    {
        if (inGame)
        {
            AllRoomList.Add(Instantiate(RoomListingPlayingPrefab, Vector3.zero, Quaternion.identity) as GameObject);
            AllRoomList[AllRoomList.Count - 1].transform.SetParent(AllGrid.transform, false);
            RoomInfo roomInfo = AllRoomList[AllRoomList.Count - 1].GetComponent<RoomInfo>();
            JoinRoomManager joinRoom = AllRoomList[AllRoomList.Count - 1].GetComponent<JoinRoomManager>();
            joinRoom.LockedRoom = lockedRoom;
            joinRoom.ErrorMessage = errorMsg;
            joinRoom.password = enterPassword;
            joinRoom.roomName = RoomNameText;
            roomInfo.SetRoomName(roomName);
            roomInfo.SetNumOfPlayers(numOfPlayers);
            roomInfo.SetRoomID(roomId);
            roomInfo.SetIsInGame(true);
            if (isLocked)
            {
                roomInfo.SetLocked(true);
            }
        }
        else
        {
            AllRoomList.Add(Instantiate(RoomListingWaitingPrefab, Vector3.zero, Quaternion.identity) as GameObject);
            AllRoomList[AllRoomList.Count - 1].transform.SetParent(AllGrid.transform, false);
            RoomInfo roomInfo = AllRoomList[AllRoomList.Count - 1].GetComponent<RoomInfo>();
            JoinRoomManager joinRoom = AllRoomList[AllRoomList.Count - 1].GetComponent<JoinRoomManager>();
            joinRoom.LockedRoom = lockedRoom;
            joinRoom.ErrorMessage = errorMsg;
            joinRoom.password = enterPassword;
            joinRoom.roomName = RoomNameText;
            roomInfo.SetRoomName(roomName);
            roomInfo.SetNumOfPlayers(numOfPlayers);
            roomInfo.SetRoomID(roomId);
            roomInfo.SetIsInGame(false);
            if (isLocked)
            {
                roomInfo.SetLocked(true);
            }
        }
    }

    public void AddToPlayingRoomList(string roomId, string roomName, bool isLocked, int numOfPlayers)
    {
        PlayingRoomList.Add(Instantiate(RoomListingPlayingPrefab, Vector3.zero, Quaternion.identity) as GameObject);
        PlayingRoomList[PlayingRoomList.Count - 1].transform.SetParent(PlayingGrid.transform, false);
        RoomInfo roomInfo = PlayingRoomList[PlayingRoomList.Count - 1].GetComponent<RoomInfo>();
        JoinRoomManager joinRoom = PlayingRoomList[PlayingRoomList.Count - 1].GetComponent<JoinRoomManager>();
        joinRoom.LockedRoom = lockedRoom;
        joinRoom.ErrorMessage = errorMsg;
        joinRoom.password = enterPassword;
        joinRoom.roomName = RoomNameText;
        roomInfo.SetRoomName(roomName);
        roomInfo.SetNumOfPlayers(numOfPlayers);
        roomInfo.SetRoomID(roomId);
        roomInfo.SetIsInGame(true);
        if (isLocked)
        {
            roomInfo.SetLocked(true);
        }
    }

    public void AddToWaitingRoomList(string roomId, string roomName, bool isLocked, int numOfPlayers)
    {
        WaitingRoomList.Add(Instantiate(RoomListingWaitingPrefab, Vector3.zero, Quaternion.identity) as GameObject);
        WaitingRoomList[WaitingRoomList.Count - 1].transform.SetParent(WaitingGrid.transform, false);
        RoomInfo roomInfo = WaitingRoomList[WaitingRoomList.Count - 1].GetComponent<RoomInfo>();
        JoinRoomManager joinRoom = WaitingRoomList[WaitingRoomList.Count - 1].GetComponent<JoinRoomManager>();
        joinRoom.LockedRoom = lockedRoom;
        joinRoom.ErrorMessage = errorMsg;
        joinRoom.password = enterPassword;
        joinRoom.roomName = RoomNameText;
        roomInfo.SetRoomName(roomName);
        roomInfo.SetNumOfPlayers(numOfPlayers);
        roomInfo.SetRoomID(roomId);
        roomInfo.SetIsInGame(false);
        if (isLocked)
        {
            roomInfo.SetLocked(true);
        }

    }

    public void UpdatePlayingRoomList(string roomId, int numOfPlayers)
    {
        for (int i = 0; i < PlayingRoomList.Count; i++)
        {
            RoomInfo roomInfo = PlayingRoomList[i].GetComponent<RoomInfo>();
            if (roomInfo.RoomID == roomId)
            {
                roomInfo.SetNumOfPlayers(numOfPlayers);
                break;
            }
        }
    }

    public void UpdateAllRoomList(string roomId, int numOfPlayers)
    {
        for (int i = 0; i < AllRoomList.Count; i++)
        {
            RoomInfo roomInfo = AllRoomList[i].GetComponent<RoomInfo>();
            if (roomInfo.RoomID == roomId)
            {
                roomInfo.SetNumOfPlayers(numOfPlayers);
                break;
            }
        }
    }

    public void UpdateWaitingRoomList(string roomId, int numOfPlayers)
    {
        for (int i = 0; i < WaitingRoomList.Count; i++)
        {
            RoomInfo roomInfo = WaitingRoomList[i].GetComponent<RoomInfo>();
            if (roomInfo.RoomID == roomId)
            {
                roomInfo.SetNumOfPlayers(numOfPlayers);
                return;
            }
        }
    }

    public void RemoveFromWaitingRoomList(string roomId)
    {
        for (int i = 0; i < WaitingRoomList.Count; i++)
        {
            RoomInfo roomInfo = WaitingRoomList[i].GetComponent<RoomInfo>();
            if (roomInfo.RoomID == roomId)
            {
                if (changeStatus)
                {
                    RemoveFromAllRoomList(roomId);

                    AddToAllRoomList(roomInfo.RoomID, roomInfo.RoomName.text, roomInfo.IsLocked, int.Parse(roomInfo.NumOfPlayers.text), true);

                    GameObject room = WaitingRoomList[i];
                    Destroy(room);
                    WaitingRoomList.Remove(room);
                    changeStatus = false;
                    return;
                }

                else
                {
                    GameObject room = WaitingRoomList[i];
                    Destroy(room);
                    WaitingRoomList.Remove(room);
                    return;
                }
            }
        }

    }

    public void RemoveFromAllRoomList(string roomId)
    {
        for (int i = 0; i < AllRoomList.Count; i++)
        {
            RoomInfo roomInfo = AllRoomList[i].GetComponent<RoomInfo>();
            if (roomInfo.RoomID == roomId)
            {
                GameObject room = AllRoomList[i];
                Destroy(room);
                AllRoomList.Remove(room);
                break;
            }
        }
    }

    public void RemoveFromPlayingRoomList(string roomId)
    {
        for (int i = 0; i < PlayingRoomList.Count; i++)
        {
            RoomInfo roomInfo = PlayingRoomList[i].GetComponent<RoomInfo>();
            if (roomInfo.RoomID == roomId)
            {
                if (changeStatus)
                {
                    RemoveFromAllRoomList(roomId);
                    AddToAllRoomList(roomInfo.RoomID, roomInfo.RoomName.text, roomInfo.IsLocked, int.Parse(roomInfo.NumOfPlayers.text), false);

                    GameObject room = WaitingRoomList[i];
                    Destroy(room);
                    PlayingRoomList.Remove(room);
                    changeStatus = false;
                    return;
                }
                else
                {
                    GameObject room = PlayingRoomList[i];
                    Destroy(room);
                    PlayingRoomList.Remove(room);
                    return;
                }

            }
        }

    }

    public void OnClickBackToLobby()
    {
        createRoomScene.SetActive(false);
        if (!AllButton.interactable)
        {
            AllList.SetActive(true);
            WaitingList.SetActive(false);
            PlayingList.SetActive(false);
            HorizontalGroup.SetActive(true);
            BG.SetActive(true);
        }
        else if (!WaitingButton.interactable)
        {
            AllList.SetActive(false);
            WaitingList.SetActive(true);
            PlayingList.SetActive(false);
            HorizontalGroup.SetActive(true);
            BG.SetActive(true);
        }
        else
        {
            AllList.SetActive(false);
            WaitingList.SetActive(false);
            PlayingList.SetActive(true);
            HorizontalGroup.SetActive(true);
            BG.SetActive(true);
        }
        exitCreateRoom = true;
    }

    public void HandleRoomCreated(Packet packet)
    {
        Debug.Log("RoomCreatedAck Packet Received");
        RoomCreatedAck ack = new RoomCreatedAck(packet);
        AddToAllRoomList(ack.Room.RoomId, ack.Room.RoomName, ack.Room.IsLocked, 1, false);
        AddToWaitingRoomList(ack.Room.RoomId, ack.Room.RoomName, ack.Room.IsLocked, 1);
    }

    public void HandleUpdateRoomPlayerCount(Packet packet)
    {
        Debug.Log("UpdateRoomPlayerCountAck Packet Received");
        UpdateRoomPlayerCountAck ack = new UpdateRoomPlayerCountAck(packet);
        if (ack.NumOfUser == 0)
        {
            Debug.Log("0 Players!!");
            changeStatus = false;
            RemoveFromAllRoomList(ack.RoomId);
            RemoveFromPlayingRoomList(ack.RoomId);
            RemoveFromWaitingRoomList(ack.RoomId);

            // Remove from WorldSelectManager.roomList?
            foreach (Room r in WorldSelectManager.roomList)
            {
                if (r.RoomId == ack.RoomId)
                {
                    WorldSelectManager.roomList.Remove(r);
                    break;
                }
            }
        }
        else
        {
            UpdateAllRoomList(ack.RoomId, ack.NumOfUser);
            UpdatePlayingRoomList(ack.RoomId, ack.NumOfUser);
            UpdateWaitingRoomList(ack.RoomId, ack.NumOfUser);
        }
    }

    public static void ChangeToRoomScene()
    {
        if (lobbyId == (int)WorldSelectManager.WorldType.Custom)
            SceneManager.LoadScene("WorldSelect");
        else
            SceneManager.LoadScene("RoomWorld" + lobbyId);
    }
}

