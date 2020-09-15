using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Network_Http
{
    public abstract class HttpRequestSender : MonoBehaviour
    {
        /// <summary>
        /// Returns the URI to query.
        /// </summary>
        /// <returns>uri; "http://<...>"</returns>
        protected abstract string GetRequestUrl();

        /// <summary>
        /// Callback on Http Request success
        /// </summary>
        /// <param name="www">Contains the request and response details</param>
        protected abstract void OnRequestSuccess(UnityWebRequest www);

        /// <summary>
        /// Callback on Http Request Bad Request
        /// </summary>
        /// <param name="www">Contains the request and response details</param>
        protected abstract void OnBadRequestError(UnityWebRequest www);

        /// <summary>
        /// Callback on Http Request Internal Server Error
        /// </summary>
        /// <param name="www">Contains the request and response details</param>
        protected abstract void OnInternalServerError(UnityWebRequest www);

        /// <summary>
        /// Used to send a Http Request. Can be via button OnClick or function callbacks, or just called inside a script.
        /// </summary>
        public void SendHttpRequest()
        {
            StartCoroutine(SendRequest());
        }

        /// <summary>
        /// Create the UnityWebRequest object.
        /// </summary>
        /// <returns>The WebRequest object</returns>
        protected abstract UnityWebRequest CreateWebRequest();

        private IEnumerator SendRequest()
        {
            UnityWebRequest www = CreateWebRequest();

            yield return www.SendWebRequest();

            switch (www.responseCode)
            {
                case HttpStatusCodes.OK:
                    Debug.Log("Request OK");
                    OnRequestSuccess(www);
                    break;
                case HttpStatusCodes.BadRequest:
                    Debug.Log("Bad Request Error: " + www.error);
                    OnBadRequestError(www);
                    break;
                case HttpStatusCodes.InternalServerError:
                default:
                    Debug.Log("Internal Server Error: " + www.error);
                    OnInternalServerError(www);
                    break;
            }
        }

    }

}
