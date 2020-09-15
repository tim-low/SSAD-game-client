using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Networking;
using SuperSad.Networking.Events;

public class SessionCreator : EventListener {

    public static string SessionCode { get; private set; }

    [SerializeField]
    private SessionList sessionList;
    [SerializeField]
    private SceneTransition sceneTransitioner;

    public override void Subscribe(INotifier<Packet> notifier)
    {
        // Subscribe to 
        notifier.Register(HandleSessionCreated, Packets.CreateSessionAck);
    }

    public void CmdCreateSession()
    {
        // Create Packet
        Packet cmd = new CmdCreateSession()
        {
            QuizId = sessionList.SelectedQuizId
        }.CreatePacket();

        // Send Packet
        NetworkStreamManager.Instance.SendPacket(cmd);
    }

    private void HandleSessionCreated(Packet packet)
    {
        CreateSessionAck ack = new CreateSessionAck(packet);

        // Save the Session Code
        SessionCode = ack.SessionCode;
        Debug.Log("Session Code: " + ack.SessionCode);

        // Change Scene
        sceneTransitioner.SceneLoader("HostWaitingPage5");
    }
}
