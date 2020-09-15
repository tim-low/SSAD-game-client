using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Network_Http
{
    public abstract class HttpRequestSenderPOST : HttpRequestSender
    {
        [SerializeField]
        protected bool uploadRawData = false;

        protected override UnityWebRequest CreateWebRequest()
        {
            UnityWebRequest www;
            if (uploadRawData)
            {
                string rawData = CreateRequestRaw();
                Debug.Log(rawData);

                www = UnityWebRequest.Post(GetRequestUrl(), rawData);
                www.method = UnityWebRequest.kHttpVerbPOST;

                www.SetRequestHeader("Content-Type", "application/json");
                www.SetRequestHeader("Accept", "application/json");
            }
            else
            {
                www = UnityWebRequest.Post(GetRequestUrl(), CreateRequestBody());
                www.SetRequestHeader("content-type", "application/x-www-form-urlencoded");
                www.chunkedTransfer = false;
            }
            return www;
        }

        // Honestly, bad practice to create functions that may not be implemented by the child classes
        protected abstract WWWForm CreateRequestBody();
        protected abstract string CreateRequestRaw();
    }
}