using Network_Http;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Post : HttpRequestSenderPOST
{

    [SerializeField]
    private Text responseText;

    protected override WWWForm CreateRequestBody()
    {
        throw new System.NotImplementedException();
    }

    protected override string CreateRequestRaw()
    {
        throw new System.NotImplementedException();
    }

    protected override string GetRequestUrl()
    {
        return "http://134.209.98.43:8000/api/v1/question/post";    // append the token
    }

    protected override void OnBadRequestError(UnityWebRequest www)
    {
        // Perform actions on unsuccessful response - request is not correct
        responseText.gameObject.SetActive(true);
        responseText.text = "Bad Request";
    }

    protected override void OnInternalServerError(UnityWebRequest www)
    {
        // Perform actions on unsuccessful response - internal server error
        responseText.gameObject.SetActive(true);
        responseText.text = "Internal Server Error";
    }

    protected override void OnRequestSuccess(UnityWebRequest www)
    {
        // Perform actions on successful response
        responseText.gameObject.SetActive(true);
        responseText.text = "Success";
    }
}