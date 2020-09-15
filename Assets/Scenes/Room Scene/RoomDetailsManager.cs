using UnityEngine;
using SuperSad.Model;
using SuperSad.Networking;
using SuperSad.Networking.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class RoomDetailsManager : EventListener
{
    [SerializeField]
    private GameObject PlayerPrefab;

    private GameObject List;

    [SerializeField]
    private GridLayoutGroup Grid;

    [SerializeField]

    private Button StartOrReadyButton;

    [SerializeField]

    private GameObject ErrorMsg;

    private List<GameObject> PlayerList = new List<GameObject>();

    public static string roomID;

    private string ownerToken;

    private bool found = false;

    private bool readyStatus = false;

    void Start()
    {
        ConstructRoomDetailsPacket();
    }

    public void SetButton()
    {
        if (UserState.Instance().Token == ownerToken)
        {
            StartOrReadyButton.GetComponentInChildren<Text>().text = "Start";
            return;
        }
        else
        {
            if (StartOrReadyButton.GetComponentInChildren<Text>().text == "Ready")
            {
                StartOrReadyButton.GetComponentInChildren<Text>().text = "Unready";
            }
            else
            {
                StartOrReadyButton.GetComponentInChildren<Text>().text = "Ready";
            }
        }
    }

    public void HandleRoomDetailsAck(Packet packet)
    {
        Debug.Log("RoomDetailsAck Packet Received");
        RoomDetailsAck ack = new RoomDetailsAck(packet);
        ownerToken = ack.OwnerToken;
        int playerCount = 0;
        foreach (Character x in ack.Characters)
        {
            PlayerList.Add(Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity) as GameObject);
            PlayerList[playerCount].transform.SetParent(Grid.transform, false);
            PlayerDetails playerDetails = PlayerList[playerCount].GetComponent<PlayerDetails>();
            playerDetails.SetPlayerName(x.Username);
            playerDetails.SetPlayerToken(x.Token);
            if (x.Token != ownerToken)
            {
                if (found)
                {
                    if (readyStatus)
                    {
                        playerDetails.SetStatus("Ready");
                    }
                    else
                    {
                        playerDetails.SetStatus("");
                    }
                    found = false;
                }
                else
                {
                    AddArrayStatus(x.Token);
                }
                playerDetails.SetHost();
            }
            else
            {
                if (found)
                {
                    playerDetails.SetStatus("Ready");
                    found = false;
                }
                else
                {
                    AddArrayStatus(x.Token);
                }
                playerDetails.SetStatus("Ready");
                StartOrReadyButton.GetComponentInChildren<Text>().text = "Start";
            }
            // Increment player count
            playerCount++;
        }
        SetButton();
    }

    public void HandleReadyStatusAck(Packet packet)
    {
        Debug.Log("ReadyStatusAck Packet Received");
        ReadyStatusAck ack = new ReadyStatusAck(packet);
        if (ack.IsReady)
        {
            UpdateStatus(ack.Token, "Ready");
        }
        else
        {
            UpdateStatus(ack.Token, "");
        }
    }

    public void UpdateLeavingPlayer(string playerToken)
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            PlayerDetails playerDetails = PlayerList[i].GetComponent<PlayerDetails>();
            if (playerDetails.Token == playerToken)
            {
                GameObject player = PlayerList[i];
                PlayerList.Remove(PlayerList[i]);
                RemoveArrayStatus(playerToken);
                Destroy(player);
                break;
            }
        }

    }

    public void HandlePlayerHasLeftRoom(Packet packet)
    {
        Debug.Log("PlayerHasLeftRoomAck Packet Received");
        PlayerHasLeftRoomAck ack = new PlayerHasLeftRoomAck(packet);
        if (ack.HasOwnerChanged)
        {
            ownerToken = ack.OwnerToken;
            UpdateOwner();
            UpdateLeavingPlayer(ack.Character.Token);
        }
        else
        {
            UpdateLeavingPlayer(ack.Character.Token);
        }

    }

    public void UpdateNewPlayer(string playerName, string playerToken)
    {
        PlayerList.Add(Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity) as GameObject);
        PlayerList[PlayerList.Count - 1].transform.SetParent(Grid.transform, false);
        PlayerDetails playerDetails = PlayerList[PlayerList.Count - 1].GetComponent<PlayerDetails>();
        playerDetails.SetPlayerName(playerName);
        playerDetails.SetPlayerToken(playerToken);
        playerDetails.SetStatus("");
        playerDetails.SetHost();
        AddArrayStatus(playerToken);

    }

    public void UpdateStatus(string playerToken, string status)
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            PlayerDetails playerDetails = PlayerList[i].GetComponent<PlayerDetails>();
            if (playerDetails.Token == playerToken)
            {
                playerDetails.SetStatus(status);
                UpdateArrayStatus(playerToken);
                break;
            }
        }
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(HandleRoomDetailsAck, Packets.RoomDetailsAck);
        notifier.Register(HandleCanStartAck, Packets.CanStartAck);
        notifier.Register(HandleReadyStatusAck, Packets.ReadyStatusAck);
        notifier.Register(HandleStartGameAck, Packets.StartGameAck);
        notifier.Register(HandleNewPlayerInRoomAck, Packets.NewPlayerInRoomAck);
        notifier.Register(HandlePlayerHasLeftRoom, Packets.PlayerHasLeftRoomAck);
    }

    public void ConstructRoomDetailsPacket()
    {

        Packet cmd = new CmdRoomDetails()
        {
            Token = UserState.Instance().Token,
            RoomId = roomID,

        }.CreatePacket();

        Debug.Log("CmdRoomDetails Packet Created");

        NetworkStreamManager.Instance.SendPacket(cmd);
    }

    public void ConstructCmdStartGamePacket()
    {
        Packet cmd = new CmdStartGame()
        {

            Token = UserState.Instance().Token,
            RoomId = roomID,

        }.CreatePacket();

        Debug.Log("CmdStartGame Packet Created");

        NetworkStreamManager.Instance.SendPacket(cmd);
        SetButton();
    }

    public void HandleCanStartAck(Packet packet)
    {
        // packet is sent only to owner
        Debug.Log("CanStartAck Packet Received");
        CanStartAck ack = new CanStartAck(packet);

        // Set Start Button to enable or disable for the host
        //StartOrReadyButton.interactable = ack.Success;
    }

    public void HandleStartGameAck(Packet packet)
    {
        Debug.Log("StartGameAck Packet Received");
        StartGameAck ack = new StartGameAck(packet);
        if (ack.Success)
        {
            // start game
            SceneManager.LoadScene("GameSession");
        }
    }

    public void HandleNewPlayerInRoomAck(Packet packet)
    {
        Debug.Log("NewPlayerInRoomAck Packet Received");
        NewPlayerInRoomAck ack = new NewPlayerInRoomAck(packet);
        UpdateNewPlayer(ack.Character.Username, ack.Character.Token);
    }


    public void UpdateOwner()
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            PlayerDetails playerDetails = PlayerList[PlayerList.Count - 1].GetComponent<PlayerDetails>();
            if (playerDetails.Token == ownerToken)
            {
                playerDetails.SetHost();
                playerDetails.SetStatus("Ready");
                UpdateArrayStatus(playerDetails.Token);
                break;
            }
        }
        if (UserState.Instance().Token == ownerToken)
        {
            SetButton();
        }

    }

    public void GetArrayStatus(string token)
    {
        found = false;
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (RoomInfo.playerTokens[i] == token)
            {
                found = true;
                readyStatus = RoomInfo.readyStates[i];
                return;
            }
        }
    }

    public void UpdateArrayStatus(string token)
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (RoomInfo.playerTokens[i] == token)
            {
                if (token == ownerToken)
                {
                    RoomInfo.readyStates[i] = true;
                    return;
                }
                RoomInfo.readyStates[i] = !RoomInfo.readyStates[i];
                return;
            }
        }
    }

    public void RemoveArrayStatus(string token)
    {
        for (int i = 0; i < PlayerList.Count; i++)
        {
            if (RoomInfo.playerTokens[i] == token)
            {
                for (int j = i; j < PlayerList.Count - 1; j++)
                {
                    RoomInfo.readyStates[j] = RoomInfo.readyStates[j + 1];
                    RoomInfo.playerTokens[j] = RoomInfo.playerTokens[j + 1];
                }
                return;
            }
        }
    }

    public void AddArrayStatus(string token)
    {
        RoomInfo.playerTokens[PlayerList.Count - 1] = token;
        if (token == ownerToken)
        {
            RoomInfo.readyStates[PlayerList.Count - 1] = true;
        }
        else
        {
            RoomInfo.readyStates[PlayerList.Count - 1] = false;
        }
    }

}



