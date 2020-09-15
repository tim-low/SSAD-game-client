using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game_Server.Model;
using SuperSad.Networking;
using SuperSad.Networking.Events;

public class AvatarLoader : EventListener
{
    private GetCharAck charAck;
    [SerializeField]
    private Text chestCountText;
    // Use this for initialization
    void Start()
    {
        ConstructGetCharPacket();

    }
    /*IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        ConstructGetCharPacket();
    }*/

    public void HandleGetChar(Packet packet)
    {
        Debug.Log("Unlock Packet Received");
        charAck = new GetCharAck(packet);
        UserState.Instance().characterWears = new byte[] { charAck.Head, charAck.Shirt, charAck.Pants, charAck.Shoes };
        chestCountText.text = (charAck.ChestCount > 9) ? "9+" : charAck.ChestCount.ToString();
        UserState.Instance().chestCount = charAck.ChestCount;
        Debug.Log("chest count: " + charAck.ChestCount);
        Debug.Log("Unlock head: " + charAck.Head);
        Debug.Log("Unlock shirt: " + charAck.Shirt);
        Debug.Log("Unlock pants: " + charAck.Pants);
        Debug.Log("Unlock shoes: " + charAck.Shoes);
    }
    public void HandleErrorPacket(Packet packet)
    {
        Debug.Log("Error Packet Received");
        ErrorAck ack = new ErrorAck(packet);
        Debug.Log(ack.Message);
    }


    public override void Subscribe(INotifier<Packet> notifier)
    {
        // for better separation of concerns (Single Responsibility Principle), these handling of packets should be in separate scripts instead of all-in-one manager like this script
        notifier.Register(HandleErrorPacket, Packets.ErrorAck);
        notifier.Register(HandleGetChar, Packets.GetCharAck);
    }

    public void ConstructGetCharPacket()
    {
        //expectError = true;
        Packet ack = new CmdGetChar()
        {
            Token = UserState.Instance().Token
        }.CreatePacket();

        Debug.Log("Get Char Packet Created");

        NetworkStreamManager.Instance.SendPacket(ack);
    }


}
