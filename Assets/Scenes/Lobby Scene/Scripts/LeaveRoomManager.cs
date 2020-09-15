using System.Collections.Generic;
using SuperSad.Model;
using SuperSad.Networking;
using SuperSad.Networking.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveRoomManager : EventListener
{

    public static string currentRoomID;

   void Start()
    {
        currentRoomID = RoomDetailsManager.roomID;
    }

    public void HandleLeaveRoom(Packet packet)
    {
        Debug.Log("LeaveRoomAck Packet Received");
        LeaveRoomAck ack = new LeaveRoomAck(packet);
        WorldSelectManager.roomList = new List<Room>(ack.Rooms);
        WorldSelectManager.reload = true;
        SceneManager.LoadScene("WorldSelect"); 
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(HandleLeaveRoom, Packets.LeaveRoomAck);
    }

    public void ConstructLeaveRoomPacket()
    {
        Packet cmd = new CmdLeaveRoom()
        {

            RoomId = currentRoomID,
            Token = UserState.Instance().Token,

        }.CreatePacket();

        Debug.Log("CmdLeaveRoom Packet Created");

        NetworkStreamManager.Instance.SendPacket(cmd);
    }
}
