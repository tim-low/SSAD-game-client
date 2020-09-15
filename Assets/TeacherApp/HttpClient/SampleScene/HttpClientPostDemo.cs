using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Network_Http;

using SuperSad.Model;

public class HttpClientPostDemo : HttpRequestSenderPOST {

    [SerializeField]
    private Text responseText;

    protected override WWWForm CreateRequestBody()
    {
        // fill up request body
        WWWForm form = new WWWForm();
        form.AddField("username", "test123");
        form.AddField("password", "testtest");
        form.AddField("items[]", "1");
        form.AddField("items[]", "2");

        return form;
    }

    protected override string CreateRequestRaw()
    {
        Answers solution = new Answers();
        string jsonData = JsonUtility.ToJson(solution);

        return jsonData;
    }

    protected override void OnRequestSuccess(UnityWebRequest www)
    {
        // Perform actions on successful response
        responseText.gameObject.SetActive(true);
        responseText.text = "Success";
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

    protected override string GetRequestUrl()
    {
        return "http://134.209.98.43:8000/api/v1/question/post";    // append the token
    }
}
