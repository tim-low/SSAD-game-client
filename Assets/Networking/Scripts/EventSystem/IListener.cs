using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace SuperSad.Networking.Events
{
    public interface IListener<T>
    {

        void Subscribe(INotifier<T> notifier);
    }

}