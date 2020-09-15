using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking
{
    public interface IPacketReceiver
    {

        // Use this for initialization
        IEnumerator Receive(Packet p);
    }
}
