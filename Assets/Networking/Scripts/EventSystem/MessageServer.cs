using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace SuperSad.Networking.Events
{
    public class MessageServer : MonoBehaviour, INotifier<Packet>, IPacketReceiver {

        //[SerializeField]
        //private List<UnityAction<Packet>> eventListeners;   // save a reference to Unsubscribe them when changing Scenes

        private Dictionary<int, PacketEvent> subscribedEvents;

        // Use this for initialization
        void Awake()
        {
            //eventListeners = new List<UnityAction<Packet>>();
            subscribedEvents = new Dictionary<int, PacketEvent>();
        }

        void Start()
        {
            //Debug.Log("MessageServer Start");

            //foreach (EventListener listener in eventListeners)
            //{
            //    listener.Subscribe(this);
            //}
            //Inject();

            // Unsubscribe when all Scenes are unloaded
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            // Remove all PacketEvents
            List<int> events = new List<int>();
            foreach (int eventId in subscribedEvents.Keys)
                events.Add(eventId);

            foreach (int eventId in events)
                subscribedEvents.Remove(eventId);

            Log.Instance.AppendLine("Unregistered Events");
        }

        /*private void Inject()
        {
            if (NetworkStreamManager.Instance != null)
            {
                Log.Instance.AppendLine("Inject");
                // assign MessageServer as Packet Receiver
                Log.Instance.AppendLine("Is NetworkStreamManager null: " + NetworkStreamManager.Instance);
                NetworkStreamManager.Instance.SetPacketReceiver(this);

                // assign NetworkStreamManager to a Packet Sender (TBC)
            }
        }*/

        public IEnumerator Receive(Packet p)
        {
            //Log.Instance.AppendLine("MessageServer Receive");
            Debug.Log("MessageServer Receive");
            Notify(p, p.Id);
            yield return null;
        }

        public void Register(UnityAction<Packet> listener, int eventId)
        {
            if (!subscribedEvents.ContainsKey(eventId))
            {
                subscribedEvents[eventId] = new PacketEvent();
            }

            subscribedEvents[eventId].AddListener(listener);
            //eventListeners.Add(listener);
        }

        public void Unregister(UnityAction<Packet> listener, int eventId)
        {
            if (subscribedEvents.ContainsKey(eventId))
            {
                subscribedEvents[eventId].RemoveListener(listener);
            }
        }

        public void Notify(Packet data, int eventId)
        {
            if (!subscribedEvents.ContainsKey(eventId))
            {
                Log.Instance.AppendLine("Event " + eventId + " does not exist");
            }
            else
            {
                Debug.Log(eventId);
                subscribedEvents[eventId].Invoke(data);
            }
        }
    }

}