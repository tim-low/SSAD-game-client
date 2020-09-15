using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;
using SuperSad.Networking;
using SuperSad.Networking.Events;

public class MessageServer_PlayModeTest {

    // Testing invoking methods
    bool invoked = false;
    private MessageServer ms = null;

    [UnityTest]
    public IEnumerator TestInvokeEvent0()
    {
        // Register a new PacketEvent
        ms.Register((Packet) => {
            Debug.Log("Invoked Event 0");
            invoked = true;
        }, 0);

        yield return null;

        // Create a Packet to invoke the Event
        Packet packet = new Packet(0);
        // Receive Packet event
        yield return ms.Receive(packet);

        // Check that invoked is true
        Assert.IsTrue(invoked);
    }

    [UnityTest]
    public IEnumerator TestMultipleListeners()
    {
        int counter = 0;
        const int numListeners = 10;

        // Register a new PacketEvent
        for (int i = 0; i < numListeners; i++)
            ms.Register((Packet) => {
                ++counter;
                Debug.Log("Invoked Event 0; counter = " + counter);
            }, 0);

        yield return null;

        // Create a Packet to invoke the Event
        Packet packet = new Packet(0);
        // Receive Packet event
        yield return ms.Receive(packet);

        // Check that there is a PacketEvent
        Assert.AreEqual(counter, numListeners);
    }

    [UnityTest]
    public IEnumerator TestOneEventListener()
    {
        GameObject gameObj = new GameObject();
        TestEventListener eventListener = gameObj.AddComponent<TestEventListener>();

        eventListener.Subscribe(ms);

        yield return null;

        // Create a Packet to invoke the Event
        Packet packet = new Packet(0);
        // Receive Packet event
        yield return ms.Receive(packet);

        // Check that EventListener was Invoked
        Assert.IsTrue(eventListener.IsInvoked);
    }

    [UnityTest]
    public IEnumerator TestMultipleEvents()
    {
        const int numEvents = 10;
        List<int> eventsInvoked = new List<int>();

        // Register a new PacketEvent
        for (int i = 0; i < numEvents; i++)
        {
            int val = i;
            ms.Register((Packet) =>
            {
                Debug.Log("Invoked Event " + val);
                eventsInvoked.Add(val);
            }, i);
        }

        yield return null;

        // Packet to invoke the Events
        for (ushort i = 0; i < numEvents; i++)
        {
            // Create a Packet to invoke the Event
            Packet packet = new Packet(i);
            // Receive Packet event
            yield return ms.Receive(packet);
        }

        // Check that events were Invoked
        Assert.AreEqual(eventsInvoked.Count, numEvents);
        for (int i = 0; i < numEvents; i++)
            Assert.AreEqual(eventsInvoked[i], i);
    }

    [UnityTest]
    public IEnumerator UnregisterEventListener()
    {
        // Create EventListener
        GameObject gameObj = new GameObject();
        TestEventListener eventListener = gameObj.AddComponent<TestEventListener>();

        // Subscribe to Event
        eventListener.Subscribe(ms);

        yield return null;

        // Unsubscribe from Event
        ms.Unregister(eventListener.TestAction, 0);

        // Create a Packet to invoke the Event
        Packet packet = new Packet(0);
        // Receive Packet event
        yield return ms.Receive(packet);

        // Check that EventListener was not Invoked
        Assert.IsFalse(eventListener.IsInvoked);
    }

    [SetUp]
    public void Setup()
    {
        // Create MessageServer
        ms = MonoBehaviour.Instantiate(Resources.Load<MessageServer>("MessageServer"));

        // Initialize to false
        invoked = false;
    }

    [TearDown]
    public void Teardown()
    {
        MonoBehaviour.DestroyImmediate(ms.gameObject);

        // Cleanup
        invoked = false;
    }
}
