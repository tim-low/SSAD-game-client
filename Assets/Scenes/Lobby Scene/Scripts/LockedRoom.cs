using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockedRoom : MonoBehaviour {

    [SerializeField]
    private InputField password;

    private JoinRoomManager currentRoomToJoin;
    private RoomInfo roomInfo;

    public void SetLockedRoomDetails(JoinRoomManager room, RoomInfo roomInfo)
    {
        currentRoomToJoin = room;
        this.roomInfo = roomInfo;
    }

    public void JoinLockedRoom()
    {
        // Send packet to attempt to join room
        currentRoomToJoin.ConstructJoinRoomPacket(roomInfo);
        // clear the password field
        password.text = "";
    }
}
