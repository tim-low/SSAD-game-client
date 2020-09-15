using UnityEngine;
using UnityEngine.UI;
using SuperSad.Networking;
using SuperSad.Networking.Events;


public class LeaveLobbyManager : EventListener
{

    [SerializeField]
    
    private GameObject lobbyScene;
    
    public void HandleLeaveLobby(Packet packet)
    {
        Debug.Log("LeaveLobbyAck Packet Received");
        LeaveLobbyAck ack = new LeaveLobbyAck(packet);
        if(ack.Success) {
            lobbyScene.SetActive(false);
        }
    }
    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(HandleLeaveLobby, Packets.LeaveLobbyAck);
    }

    public void ConstructLeaveLobbyPacket()
    {
        Packet cmd = new CmdLeaveLobby()
        {
           
            Token = UserState.Instance().Token,

        }.CreatePacket();

        Debug.Log("CmdLeaveLobby Packet Created");

        NetworkStreamManager.Instance.SendPacket(cmd);
    }

}