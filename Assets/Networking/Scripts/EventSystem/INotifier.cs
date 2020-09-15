using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SuperSad.Networking.Events
{
    public interface INotifier<T> {

        // Subscribe an Action as a Listener to an event
        void Register(UnityAction<T> listener, int eventId);
        // Unsubscribe an Action as a Listener to an event
        void Unregister(UnityAction<T> listener, int eventId);
        // Notify IListeners to an event
        void Notify(T data, int eventId);
    }

}