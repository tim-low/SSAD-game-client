using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Network_Http
{
    public abstract class HttpRequestSenderGET : HttpRequestSender {

        [Tooltip("Params to append to the request URL")]
        protected string[] parameters;

        protected override UnityWebRequest CreateWebRequest()
        {
            bool firstParam = true;
            string uri = GetRequestUrl();
            if (parameters != null)
            {
                foreach (string param in parameters)
                {
                    string value = SetParameterValue(param);
                    AppendParam(uri, param, value, firstParam);
                    if (firstParam)
                        firstParam = false;
                }
            }
            UnityWebRequest www = UnityWebRequest.Get(uri);
            return www;
        }

        protected abstract string SetParameterValue(string param);

        public static string AppendParam(string uri, string key, string value, bool firstParam)
        {
            uri += firstParam ? "?" : "&";
            uri += key + "=" + value;

            return uri;
        }
    }

}