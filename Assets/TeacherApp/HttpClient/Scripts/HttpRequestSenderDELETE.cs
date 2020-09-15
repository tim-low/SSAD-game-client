using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Network_Http
{
    public abstract class HttpRequestSenderDELETE : HttpRequestSender
    {
        protected override UnityWebRequest CreateWebRequest()
        {
            string rawData = CreateRequestRaw();
            Debug.Log(rawData);

            // Create the UnityWebRequest
            UnityWebRequest www = UnityWebRequest.Put(GetRequestUrl(), rawData);    // create as PUT first
            www.method = UnityWebRequest.kHttpVerbDELETE;   // set the method to DELETE

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");

            return www;
        }

        protected abstract string CreateRequestRaw();
    }
}