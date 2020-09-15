using UnityEngine;
using SuperSad.Model;
using SuperSad.Networking;
using SuperSad.Networking.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class JoinRoomManager : EventListener
{


    [SerializeField]
    public InputField password;

    [SerializeField]
    public RoomInfo selectedRoom;

    [SerializeField]
    public Text roomName;

    [SerializeField]

    public GameObject LockedRoom;

    [SerializeField]

    public GameObject ErrorMessage;

    /*public void HandleErrorPacket(Packet packet)
    {
        ErrorAck ack = new ErrorAck(packet);
        ErrorMessage.SetActive(true);
        ErrorMessage.GetComponentInChildren<Text>().text = ack.Message;
    }*/

    public void HandleJoinRoom(Packet packet)
    {
        Debug.Log("JoinRoomAck Packet Received");
        JoinRoomAck ack = new JoinRoomAck(packet);
        RoomDetailsManager.roomID = ack.Room.RoomId;
        //SceneManager.LoadScene("RoomScene");   // TEMPORARY
        RoomListingDisplayManager.ChangeToRoomScene();
    }

    public void ConstructJoinRoomPacket(RoomInfo selectedRoom)
    {
        Packet cmd;
        if (selectedRoom.IsInGame)
        {
            ErrorMessage.SetActive(true);
            GameObject ErrorText = ErrorMessage.transform.GetChild(0).gameObject;
            ErrorText.GetComponentInChildren<Text>().text = "Room is in game, select another room";
            return;
        }
        if (selectedRoom.IsLocked)
        {
            if (password.text == "")
            {
                if (!LockedRoom.activeSelf)
                {
                    LockedRoom.SetActive(true);
                    roomName.text = selectedRoom.RoomName.text.ToString();

                    // Set Locked Room
                    LockedRoom.GetComponent<LockedRoom>().SetLockedRoomDetails(this, selectedRoom);
                    return;
                }
                else
                {
                    ErrorMessage.SetActive(true);
                    GameObject ErrorText = ErrorMessage.transform.GetChild(0).gameObject;
                    ErrorText.GetComponentInChildren<Text>().text = "Enter Password";
                    return;
                }

            }
            cmd = new CmdJoinRoom()
            {
                IsLocked = true,
                RoomId = selectedRoom.RoomID,
                Token = UserState.Instance().Token,
                Password = Sha1Sum2(password.text),
            }.CreatePacket();

        }
        else
        {
            cmd = new CmdJoinRoom()
            {
                IsLocked = false,
                RoomId = selectedRoom.RoomID,
                Token = UserState.Instance().Token,
            }.CreatePacket();
        }

        Debug.Log("CmdJoinRoom Packet Created");

        NetworkStreamManager.Instance.SendPacket(cmd);
    }
    public static string Sha1Sum2(string str)
    {
        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        byte[] bytes = encoding.GetBytes(str);
        var sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
        return System.BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", string.Empty).ToLower();
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(HandleJoinRoom, Packets.JoinRoomAck);
        //notifier.Register(HandleErrorPacket, Packets.ErrorAck);
    }

    public void OnClickBack()
    {
        LockedRoom.SetActive(false);
    }

}