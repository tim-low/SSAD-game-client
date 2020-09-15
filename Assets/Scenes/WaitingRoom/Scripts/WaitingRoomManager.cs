using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Networking;
using SuperSad.Networking.Events;
using SuperSad.Model;

using SuperSad.Gameplay;

public class WaitingRoomManager : PlayerManager {

    [SerializeField]
    private GameObject waitingRoomCanvas;
    [SerializeField]
    private Transform playerLayoutGroup;
    [SerializeField]
    private ListItemPlayer playerItemPrefab;
    [SerializeField]
    private Sprite[] headIcon;

    [SerializeField]
    private ManageScene manageScene;

    public override void Subscribe(INotifier<Packet> notifier)
    {
        base.Subscribe(notifier);

        // Subscribe to this Player joining the Session event
        notifier.Register(HandleJoinSession, Packets.JoinSessionAck);
        // Subscribe to other Players leaving the Session event
        notifier.Register(HandleLeaveSession, Packets.LeaveSessionAck);
        
        // Subscribe to other Players joining the Session event
        notifier.Register(HandlePlayerJoinedSession, Packets.ClientPlayerJoinedSessionAck);
        // Subscribe to other Players leaving the Session event
        notifier.Register(HandlePlayerLeftSession, Packets.ClientPlayerLeftSessionAck);

        // Subscribe to start session event
        notifier.Register(SessionStarted, Packets.StartSessionAck);
        // Subscribe to cancel session event
        notifier.Register(SessionCancelled, Packets.CancelSessionAck);
    }

    private void HandleJoinSession(Packet packet)
    {
        JoinSessionAck ack = new JoinSessionAck(packet);

        // Show Waiting Room canvas
        waitingRoomCanvas.SetActive(true);
        // Set Id
        RoomListingDisplayManager.lobbyId = (int)WorldSelectManager.WorldType.Custom;

        foreach (WaitingRoomUser user in ack.WaitingRoomUsers)
        {
            SpawnPlayer(user);
        }
    }
    private void HandlePlayerJoinedSession(Packet packet)
    {
        ClientPlayerJoinedSessionAck ack = new ClientPlayerJoinedSessionAck(packet);

        SpawnPlayer(ack.User);
    }

    private void SpawnPlayer(WaitingRoomUser newUser)
    {
        if (!ContainsPlayer(newUser.Token))
        {
            // Create new Player listing
            ListItemPlayer newPlayer = Instantiate(playerItemPrefab);
            newPlayer.transform.SetParent(playerLayoutGroup);
            newPlayer.gameObject.SetActive(true);
            newPlayer.transform.localScale = new Vector3(1f, 1f, 1f);

            // Set Player details
            newPlayer.SetPlayerDetails(newUser.Username, Color.white, headIcon[newUser.Head]);

            // Add to Dictionary
            this.AddPlayer(newUser.Token, newPlayer);
        }
        else
        {
            Log.Instance.AppendLine("User " + newUser.Username + " already exists!");
        }
    }

    private void HandleLeaveSession(Packet packet)
    {
        //LeaveSessionAck ack = new LeaveSessionAck(packet);

        // Delete all existing Players

        // Close Waiting Room canvas
        waitingRoomCanvas.SetActive(false);
    }
    private void HandlePlayerLeftSession(Packet packet)
    {
        ClientPlayerLeftSessionAck ack = new ClientPlayerLeftSessionAck(packet);

        // Delete the player that left
        if (ContainsPlayer(ack.Token))
        {
            BasePlayer despawned = GetPlayer(ack.Token);
            Destroy(despawned.gameObject);

            // Remove Player from list
            this.RemovePlayer(ack.Token);
        }
        else
        {
            Log.Instance.AppendLine("User " + ack.Token + " does not exist!");
        }
    }

    private void SessionStarted(Packet packet)
    {
        StartSessionAck ack = new StartSessionAck(packet);

        if (ack.Success && waitingRoomCanvas.gameObject.activeSelf)
        {
            manageScene.LoadScene("GameSession");
        }
        else
        {
            Log.Instance.AppendLine("Session start was attempted, but unsuccessful");
        }
    }
    private void SessionCancelled(Packet packet)
    {
        CancelSessionAck ack = new CancelSessionAck(packet);

        if (ack.Cancelled && waitingRoomCanvas.gameObject.activeSelf)
        {
            waitingRoomCanvas.SetActive(false);
            ClearPlayerList();
        }
        else
        {
            Log.Instance.AppendLine("Session start was attempted, but unsuccessful");
        }
    }

    private void ClearPlayerList()
    {

    }
}
