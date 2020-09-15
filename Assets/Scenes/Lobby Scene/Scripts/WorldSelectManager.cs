using System.Collections.Generic;
using SuperSad.Model;
using SuperSad.Networking;
using SuperSad.Networking.Events;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelectManager : EventListener
{
    [SerializeField]
    private GameObject lobbyScene;
    [SerializeField]
    private Text LobbyNameText;
    [SerializeField]
    private Text worldTitleText;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private string[] descriptionString;
    //[SerializeField]
    private static WorldType worldType;

    public static List<Room> roomList = new List<Room>();

    public static bool reload = false;

    //public static string worldName;

    void Start()
    {
        //if (reload && RoomListingDisplayManager.lobbyId == (int)worldType)
        if (reload)
        {
            reload = false;
            lobbyScene.SetActive(true);
            LobbyNameText.text = worldType.ToString();
        }
    }

    public enum WorldType
    {
        Requirements = 1,
        Design = 2,
        Implementation = 3,
        Testing = 4,
        Maintenance = 5,
        Miscellaneous = 6,
        Custom = 7
    }

    public void setWorldType(int worldType)
    {
        WorldSelectManager.worldType = (WorldType)worldType;
        worldTitleText.text = WorldSelectManager.worldType.ToString();
        descriptionText.text = descriptionString[worldType-1];
    }

    public void HandleErrorPacket(Packet packet)
    {
        ErrorAck ack = new ErrorAck(packet);
        Debug.Log(ack.Message);
    }

    public void HandleWorldSelectAckPacket(Packet packet)
    {
        if ((int)worldType == RoomListingDisplayManager.lobbyId)
        {
            Log.Instance.AppendLine("WorldSelectAck Packet Received");
            WorldSelectAck ack = new WorldSelectAck(packet);

            foreach (Room room in ack.Rooms)
                Debug.Log("Room: " + room.RoomName);

            roomList = new List<Room>(ack.Rooms);
            Debug.Log("TEFAWFAWFD" + worldType.ToString());
            LobbyNameText.text = worldType.ToString();
            lobbyScene.SetActive(true);
			AudioManager.instance.PlaySFX ("UIPop");
        }
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(HandleErrorPacket, Packets.ErrorAck);
        notifier.Register(HandleWorldSelectAckPacket, Packets.WorldSelectAck);
    }

    public void ConstructWorldSelectPacket()
    {
    
        RoomListingDisplayManager.lobbyId = (int)worldType;

        Packet cmd = new CmdWorldSelect()
        {
            LobbyId = (int)worldType,
            Token = UserState.Instance().Token,

        }.CreatePacket();

        Debug.Log("CmdWorldSelect Packet Created");

        NetworkStreamManager.Instance.SendPacket(cmd);
    }

}