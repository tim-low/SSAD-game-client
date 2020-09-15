using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SuperSad.Networking.Events;
using SuperSad.Networking;

public class Log : EventListener {

    [SerializeField]
    private Text logText;
    [SerializeField]
    private int maxLines = 25;

    private int numLines = 0;

    public static Log Instance
    {
        get; private set;
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        // Clear the Log first
        Clear();
    }

    void Start() {
        //SceneManager.sceneLoaded += SubscribeToMessageServer;
    }

    private void SubscribeToMessageServer(Scene scene, LoadSceneMode mode) {
        MessageServer ms = GameObject.Find("MessageServer").GetComponent<MessageServer>();
        Subscribe(ms);
    }

    public void AppendLine(string line)
    {
        numLines += 1;
        logText.text += line + "\n";
        if (numLines > maxLines)
        {
            int charLoc = logText.text.IndexOf('\n');
            logText.text = logText.text.Substring(charLoc + 1, logText.text.Length - charLoc - 1);
            numLines = maxLines;
        }

        // Print in Console as well
        Debug.Log(line);
    }

    public IEnumerator AppendLineFromOtherThread(string line)
    {
        yield return null;
        AppendLine(line);
    }

    public void Clear()
    {
        logText.text = "";
        numLines = 0;
    }

    public static void WriteLine(string line)
    {
        Debug.Log(line);
    }

    public static void WriteLine(string str, params object[] args)
    {
        Debug.Log(string.Format(str, args));
    }

    public override void Subscribe(INotifier<Packet> notifier)
    {
        // Subscribe to Error Ack packet event
        notifier.Register(OnErrorAck, Packets.ErrorAck);
    }

    private void OnErrorAck(Packet packet)
    {
        ErrorAck ack = new ErrorAck(packet);

        AppendLine(ack.Message);
    }
}
