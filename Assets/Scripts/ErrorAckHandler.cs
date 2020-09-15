using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Networking.Events;
using SuperSad.Networking;

public class ErrorAckHandler : EventListener {

    [SerializeField]
    private Text errorText;

    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(HandleErrorAck, Packets.ErrorAck);
    }

    private void HandleErrorAck(Packet packet)
    {
        ErrorAck ack = new ErrorAck(packet);
        errorText.text = ack.Message;

        this.gameObject.SetActive(true);
    }
}
