using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game_Server.Model;
using SuperSad.Networking;
using SuperSad.Networking.Events;

public class AvatarManager : EventListener {
    private GetUnlocksAck unlocksAck;
    private GetCharAck charAck;
    private BitArray headFlags;
    private BitArray shirtFlags;
    private BitArray pantsFlags;
    private BitArray shoesFlags;
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private GameObject msgWindow;
    // Use this for initialization
    void Start () {
        ConstructGetUnlocksPacket();
    }


    public void HandleUnlocks(Packet packet)
    {
        Debug.Log("Unlock Packet Received");
        unlocksAck = new GetUnlocksAck(packet);
        headFlags = new BitArray(new byte[] { unlocksAck.Head });
        shirtFlags = new BitArray(new byte[] { unlocksAck.Shirt });
        pantsFlags = new BitArray(new byte[] { unlocksAck.Pants });
        shoesFlags = new BitArray(new byte[] { unlocksAck.Shoes });
        inventory.loadData(headFlags, shirtFlags, pantsFlags, shoesFlags);
        Debug.Log("Unlock head: " + unlocksAck.Head);
        Debug.Log("Unlock shirt: " + unlocksAck.Shirt);
        Debug.Log("Unlock pants: " + unlocksAck.Pants);
        Debug.Log("Unlock shoes: " + unlocksAck.Shoes);
    }

    public void HandleErrorPacket(Packet packet)
    {
        Debug.Log("Error Packet Received");
        ErrorAck ack = new ErrorAck(packet);
        Debug.Log(ack.Message);
    }

    /*public void HandleUpdateUnlockPacket(Packet packet)
    {
        Debug.Log("update unlock Packet Received");
        UpdateUnlocksAck ack = new UpdateUnlocksAck(packet);
        Debug.Log(ack.Success);
    }*/

    public void HandleUpdateCharPacket(Packet packet)
    {
        Debug.Log("update unlock Packet Received");
        msgWindow.SetActive(true);
        UpdateCharAck ack = new UpdateCharAck(packet);
        Debug.Log(ack.Token);
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        // for better separation of concerns (Single Responsibility Principle), these handling of packets should be in separate scripts instead of all-in-one manager like this script
        notifier.Register(HandleUnlocks, Packets.GetUnlocksAck);
        notifier.Register(HandleErrorPacket, Packets.ErrorAck);
        //notifier.Register(HandleGetChar, Packets.GetCharAck);
        //notifier.Register(HandleUpdateUnlockPacket, Packets.UpdateUnlocksAck);
        notifier.Register(HandleUpdateCharPacket, Packets.UpdateCharAck);
    }
    public void ConstructGetUnlocksPacket()
    {
        Packet ack = new CmdGetUnlocks()
        {
            Token = UserState.Instance().Token
        }.CreatePacket();

        Debug.Log("Get Unlocks Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
    }


    /*public void ConstructUpdateUnlocksPacket()
    {
        Packet ack = new CmdUpdateUnlocks()
        {
            Token = UserState.Instance().Token,
            Head = 255,
            Shirt = 2,
            Pants = 3,
            Shoes = 10

        }.CreatePacket();

        Debug.Log("Get Update Unlocks Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
    }*/

    public void ConstructUpdateCharPacket()
    {
        Packet ack = new CmdUpdateChar()
        {
            Token = UserState.Instance().Token,
            Head = UserState.Instance().characterWears[0],
            Shirt = UserState.Instance().characterWears[1],
            Pant = UserState.Instance().characterWears[2],
            Shoe = UserState.Instance().characterWears[3]

        }.CreatePacket();

        Debug.Log("Get Update char Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
    }
}
