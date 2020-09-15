using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperSad.Networking.Events;
using SuperSad.Networking;

public class JoinSessionManager : EventListener {

    [SerializeField]
    private GameObject joinSessionCanvas;
    [SerializeField]
    private InputField sessionCodeInputField;
    [SerializeField]
    private Text quizNameText;
    [SerializeField]
    private Text sessionCodeText;
    [SerializeField]
    private string sessionCodePrefix = "Code: ";

    private string sessionCode = "";

    public override void Subscribe(INotifier<Packet> notifier)
    {
        // Subscribe to JoinSessionAck
        notifier.Register(JoinedSession, Packets.JoinSessionAck);

        // Subscribe to LeaveSessionAck
        notifier.Register(LeftSession, Packets.LeaveSessionAck);
    }

    private void JoinedSession(Packet packet)
    {
        JoinSessionAck ack = new JoinSessionAck(packet);

        // Set quiz name
        quizNameText.text = ack.QuizName;
        // Set session code
        sessionCodeText.text = sessionCodePrefix + sessionCode;

        // Disable join session canvas
        joinSessionCanvas.SetActive(false);
    }
    private void LeftSession(Packet packet)
    {
        // Reset saved Session Code
        sessionCode = "";
    }

    public void SendSessionCode()
    {
        // Get input session code
        sessionCode = sessionCodeInputField.text;

        // Reset InputField
        sessionCodeText.text = "";

        // Create Packet
        Packet cmd = new CmdJoinSession()
        {
            SessionCode = sessionCode
        }.CreatePacket();

        // Send Packet
        NetworkStreamManager.Instance.SendPacket(cmd);
    }

    public void CmdLeaveSession()
    {
        // Create Packet
        Packet cmd = new CmdLeaveSession()
        {
            SessionCode = sessionCode
        }.CreatePacket();

        // Send Packet
        NetworkStreamManager.Instance.SendPacket(cmd);
    }
}
