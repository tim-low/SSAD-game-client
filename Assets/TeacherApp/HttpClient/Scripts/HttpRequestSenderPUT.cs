using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Network_Http
{
    public abstract class HttpRequestSenderPUT : HttpRequestSender
    {
        protected override UnityWebRequest CreateWebRequest()
        {
            string rawData = CreateRequestRaw();
            Debug.Log(rawData);

            // Create the UnityWebRequest
            UnityWebRequest www = UnityWebRequest.Put(GetRequestUrl(), rawData);

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");

            return www;
        }

        protected abstract string CreateRequestRaw();
    }
}