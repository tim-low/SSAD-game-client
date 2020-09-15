using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking.Events
{
    public abstract class EventListener : MonoBehaviour, IListener<Packet>
    {
        private MessageServer messageServer = null; // reference can be kept for unsubscribing

        private void Awake()
        {
            /*messageServer = GameObject.FindGameObjectWithTag("MessageServer").GetComponent<MessageServer>();
            if (messageServer == null)
            {
                Log.Instance.AppendLine("MessageServer not found by EventListener " + gameObject.name);
            }
            else
            {
                // Subscribe to packet events
                this.Subscribe(messageServer);
                Debug.Log(gameObject.name + " subscribed to MessageServer");
            }*/

            // Call Init
            Init();
        }

        protected virtual void Init() { }

        public abstract void Subscribe(INotifier<Packet> notifier);
    }

}