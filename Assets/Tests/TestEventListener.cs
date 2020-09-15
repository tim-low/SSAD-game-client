using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Networking.Events;
using SuperSad.Networking;

public class TestEventListener : EventListener {

    public bool IsInvoked { get; private set; }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        notifier.Register(TestAction, 0);

        //throw new System.NotImplementedException();
    }

    public void TestAction(Packet p)
    {
        IsInvoked = true;
    }
}
