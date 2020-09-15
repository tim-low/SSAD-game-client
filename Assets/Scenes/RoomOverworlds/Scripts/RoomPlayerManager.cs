using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Networking.Events;
using SuperSad.Networking;
using SuperSad.Model;
using SuperSad.Gameplay;

public class RoomPlayerManager : PlayerManager {

    [SerializeField]
    private RoomPlayer roomPlayerPrefab;
    [SerializeField]
    [Header("Suitable Color for username display based on environment")]
    private Color usernameColor;

    public override void Subscribe(INotifier<Packet> notifier)
    {
        base.Subscribe(notifier);

        // Subscribe to CreateRoomAck & JoinRoomAck packet event
        //notifier.Register(InitializeRoom, Packets.CreateRoomAck);
        //notifier.Register(InitializeRoom, Packets.JoinRoomAck);

        // Subscribe to RoomDetailsAck packet event
        notifier.Register(PopulateRoom, Packets.RoomDetailsAck);

        // Subscribe to RoomDetailsAck packet event
        notifier.Register(NewPlayerInRoom, Packets.NewPlayerInRoomAck);

        // Subscribe to RoomPlayerMoveAck packet event
        notifier.Register(RoomPlayerMoveAck, Packets.RoomPlayerMoveAck);

        // Subscribe to DespawnPlayerAck packet event
        notifier.Register(DespawnPlayer, Packets.PlayerHasLeftRoomAck);
    }

    public RoomPlayer GetRoomPlayer(string token)
    {
        return (RoomPlayer)GetPlayer(token);
    }

    private void RoomPlayerMoveAck(Packet packet)
    {
        RoomPlayerMoveAck ack = new RoomPlayerMoveAck(packet);

        // Move Player
        MovePlayer(ack.Token, ack.StartPos, ack.TargetPos);
    }

    public void MovePlayer(string token, Vector3 startPos, Vector3 targetPos)
    {
        RoomPlayer player = (RoomPlayer)GetPlayer(token);
        if (player != null)
        {
            player.MoveToTargetPos(startPos, targetPos);
        }
    }

    /*private void InitializeRoom(Packet packet)
    {
        CreateRoomAck ack = new CreateRoomAck(packet);  // Join Room and Create Room are the same data model

        Log.Instance.AppendLine("InitializeRoom");
        Log.Instance.AppendLine("Send CmdRoomDetails: " + UserState.Instance().Token + " | " + ack.Room.RoomId);

        // Send CmdRoomDetails
        Packet cmd = new CmdRoomDetails
        {
            Token = UserState.Instance().Token,
            RoomId = ack.Room.RoomId
        }.CreatePacket();

        // Send Packet
        NetworkStreamManager.Instance.SendPacket(cmd);
    }*/

    private void PopulateRoom(Packet packet)
    {
        RoomDetailsAck ack = new RoomDetailsAck(packet);
        Log.Instance.AppendLine("RoomDetailsAck");

        // Spawn Player
        foreach (Character character in ack.Characters)
        {
            // Identify Owner
            SpawnPlayer(character, (character.Token == ack.OwnerToken));
        }
    }

    private void DespawnPlayer(Packet packet)
    {
        //DespawnPlayerAck ack = new DespawnPlayerAck(packet);
        PlayerHasLeftRoomAck ack = new PlayerHasLeftRoomAck(packet);

        if (ContainsPlayer(ack.Character.Token))
        {
            // Destroy GameObject
            RoomPlayer roomPlayer = GetRoomPlayer(ack.Character.Token);
            Destroy(roomPlayer.gameObject);

            // Remove from player list
            RemovePlayer(ack.Character.Token);

            // Update Owner
            if (ack.HasOwnerChanged)
            {
                // Get new Owner
                RoomPlayer newOwnerPlayer = GetRoomPlayer(ack.OwnerToken);
                newOwnerPlayer.SetRoomOwner(true);
            }
        }
        else
        {
            Log.Instance.AppendLine(ack.Character.Token + " does not exist");
        }
    }

    private void SpawnPlayer(Character character, bool isOwner = false)
    {
        if (!ContainsPlayer(character.Token))
        {
            // Create GamePlayer object
            RoomPlayer roomPlayer = Instantiate(roomPlayerPrefab);

            // Set GamePlayer world position
            roomPlayer.transform.position = character.Pos;
            // Set rotation
            roomPlayer.RotateCharacter(character.Dir);

            // Set Character Details
            roomPlayer.SetCharacterDetails(character, usernameColor);
            roomPlayer.SetRoomOwner(isOwner);

            // Update Player List
            AddPlayer(character.Token, roomPlayer);
        }
        else
        {
            Log.Instance.AppendLine(character.Username + " already exists");
        }
    }

    private void NewPlayerInRoom(Packet packet)
    {
        NewPlayerInRoomAck ack = new NewPlayerInRoomAck(packet);

        Log.Instance.AppendLine("NewPlayerInRoomAck");
        SpawnPlayer(ack.Character);
    }

}
