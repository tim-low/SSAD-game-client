using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Networking;
using SuperSad.Networking.Events;

public class HostedSessionManager : EventListener {

    [Header("Initialization")]
    [SerializeField]
    private Text sessionCodeText;
    [SerializeField]
    private string sessionCodeString = "SessionCode: ";
    [SerializeField]
    private Text numParticipantsText;
    [SerializeField]
    private string numParticipantsString = "Participants: ";

    [Header("Participants List")]
    [SerializeField]
    private GameObject participantNamePrefab;
    [SerializeField]
    private Transform participantsLayoutGroup;

    [Header("Success Panel")]
    [SerializeField]
    private GameObject responsePanel;
    [SerializeField]
    private Text responsePanelText;
    [SerializeField]
    private Button responsePanelButton;

    [Header("Others")]
    [SerializeField]
    private SceneTransition sceneTransitioner;

    private Dictionary<string, GameObject> participantNames;
    private int numParticipants = 0;

    public override void Subscribe(INotifier<Packet> notifier)
    {
        // Subscribe to player joining session event
        notifier.Register(PlayerJoinedSession, Packets.TeacherPlayerJoinedSessionAck);
        // Subscribe to player leaving session event
        notifier.Register(PlayerLeftSession, Packets.TeacherPlayerLeftSessionAck);

        // Subscribe to start session event
        notifier.Register(SessionStarted, Packets.StartSessionAck);
        // Subscribe to cancel session event
        notifier.Register(SessionCancelled, Packets.CancelSessionAck);
    }

    // Use this for initialization
    void Awake() {
        // initialize variables
        participantNames = new Dictionary<string, GameObject>();
        numParticipants = 0;

        // initialize session UI
        sessionCodeText.text = sessionCodeString + SessionCreator.SessionCode;
        numParticipantsText.text = numParticipantsString + "0";

        // Disable success panel first
        responsePanel.SetActive(false);
    }
	
    public void CmdStartSession()
    {
        // Create Packet
        Packet cmd = new CmdStartSession()
        {
            SessionCode = SessionCreator.SessionCode
        }.CreatePacket();

        // Send Packet
        NetworkStreamManager.Instance.SendPacket(cmd);
    }

    public void CmdCancelSession()
    {
        // Create Packet
        Packet cmd = new CmdCancelSession()
        {
            SessionCode = SessionCreator.SessionCode
        }.CreatePacket();

        // Send Packet
        NetworkStreamManager.Instance.SendPacket(cmd);
    }

    private void PlayerJoinedSession(Packet packet)
    {
        TeacherPlayerJoinedSessionAck ack = new TeacherPlayerJoinedSessionAck(packet);

        // add new player
        if (!participantNames.ContainsKey(ack.StudentName)) // is new player
        {
            // increment participant list
            numParticipants++;
            numParticipantsText.text = numParticipantsString + numParticipants.ToString();

            // create participant name
            GameObject participant = Instantiate(participantNamePrefab);
            participant.transform.SetParent(participantsLayoutGroup);
            participant.SetActive(true);
            participant.transform.localScale = new Vector3(1f, 1f, 1f);

            // set name
            participant.transform.Find("Text").GetComponent<Text>().text = ack.StudentName;

            // add to Dictionary
            participantNames.Add(ack.StudentName, participant);
        }
        else
        {
            Debug.Log(ack.StudentName + " already exists");
        }
    }

    private void PlayerLeftSession(Packet packet)
    {
        TeacherPlayerLeftSessionAck ack = new TeacherPlayerLeftSessionAck(packet);
        
        // remove player
        if (participantNames.ContainsKey(ack.StudentName)) // is in player list
        {
            // decrement participant list
            --numParticipants;
            numParticipantsText.text = numParticipantsString + numParticipants.ToString();

            // get participant
            GameObject participant = participantNames[ack.StudentName];

            // delete participant
            Destroy(participant);

            // remove from Dictionary
            participantNames.Remove(ack.StudentName);
        }
        else
        {
            Debug.Log(ack.StudentName + " not found");
        }
    }

    private void SessionStarted(Packet packet)
    {
        StartSessionAck ack = new StartSessionAck(packet);

        if (ack.Success)
        {
            Debug.Log("Session Started");

            responsePanel.SetActive(true);
            responsePanelText.text = "Session started successfully";
            responsePanelButton.onClick.AddListener(() => sceneTransitioner.SceneLoader("MainMenuScene1"));
        }
        else
        {
            Debug.Log("Start Session Failed");

            responsePanel.SetActive(true);
            responsePanelText.text = "Session failed to start";
        }
    }

    private void SessionCancelled(Packet packet)
    {
        CancelSessionAck ack = new CancelSessionAck(packet);

        if (ack.Cancelled)
        {
            Debug.Log("Session Cancelled");

            // change scene
            sceneTransitioner.SceneLoader("HostCustomQuizScene4");
        }
        else
        {
            Debug.Log("Session was not cancelled");

            responsePanel.SetActive(true);
            responsePanelText.text = "Session failed to cancel";
            responsePanelButton.onClick.AddListener(() => sceneTransitioner.SceneLoader("HostCustomQuizScene4"));
        }
    }
}
