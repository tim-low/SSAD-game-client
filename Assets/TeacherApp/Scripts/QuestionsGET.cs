using Network_Http;
using SuperSad.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuestionsGET : HttpRequestSenderGET
{
    //[SerializeField] private string token;
    [SerializeField] private Text responseText;

    string questionsJson;
    bool success = true;

    public void loadQuestions()
    {
        SendHttpRequest();
        if(success)
        {
            Debug.Log(questionsJson);

        }
    }

    protected override string GetRequestUrl()
    {
        return "http://134.209.98.43:8000/api/v1/question/get/" + UserState.Instance().Token; //appended with token
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
        success = true;
        questionsJson = www.downloadHandler.text;
        Debug.Log(www.downloadHandler.text);
    }

    protected override string SetParameterValue(string param)
    {
        throw new System.NotImplementedException();
    }
}