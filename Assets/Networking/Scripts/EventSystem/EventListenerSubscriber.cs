using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking.Events
{
    public class EventListenerSubscriber : MonoBehaviour {

        [SerializeField]
        private EventListener[] eventListeners;

        //[SerializeField]
        //private MessageServer messageServer;

        void Awake()
        {
            MessageServer messageServer = GameObject.FindGameObjectWithTag("MessageServer").GetComponent<MessageServer>();
            foreach (EventListener listener in eventListeners)
            {
                Debug.Log("Registered " + listener.gameObject.name);
                listener.Subscribe(messageServer);
            }
        }
    }

}