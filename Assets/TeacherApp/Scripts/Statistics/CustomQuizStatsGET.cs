using Network_Http;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using SuperSad.Model;

public class CustomQuizStatsGET : HttpRequestSenderGET
{
    [SerializeField] Text responseText;
    private bool GETsuccess = false;
    private string customQuizStatsJson;
    public CustomQuizzesStats cusQuizzesStats;
    [SerializeField] List<TableDisplay> tableDisplay;
    [SerializeField] List<GraphDisplay> graphDisplay;
    [SerializeField] List<Dropdownmanager> quizDropdowns;
    [SerializeField] List<Dropdownmanager> questionDropdowns;


    void Start()
    {
        LoadCustomQuizzesStats();

    }
    public void LoadCustomQuizzesStats()
    {
        SendHttpRequest();
        if(GETsuccess)
        {
/*            Debug.Log(customQuizStatsJson);
           //cusQuizzesStats = JsonUtility.FromJson<CustomQuizzesStats>(customQuizStatsJson);
            Debug.Log(cusQuizzesStats.quizStat.Length);*/

            //display Data
        }
    }

    protected override string GetRequestUrl()
    {
        return "http://134.209.98.43:8000/api/v1/analytic/quiz/" + UserState.Instance().Token;
    }

    protected override void OnBadRequestError(UnityWebRequest www)
    {
        // Perform actions on unsuccessful response - request is not correct
        responseText.text = "Bad Request";
    }

    protected override void OnInternalServerError(UnityWebRequest www)
    {
        // Perform actions on unsuccessful response - internal server error
        responseText.text = "Internal Server Error";
    }

    protected override void OnRequestSuccess(UnityWebRequest www)
    {
        // Perform actions on successful response
        responseText.text = "Success";
        GETsuccess = true;
        customQuizStatsJson = www.downloadHandler.text;
        Debug.Log(www.downloadHandler.text);
        cusQuizzesStats = JsonUtility.FromJson<CustomQuizzesStats>(customQuizStatsJson);
        Debug.Log(cusQuizzesStats.quizStat.Length);
        if (graphDisplay != null)
            for (int i = 0; i < graphDisplay.Count; i++)
            { 
                graphDisplay[i].GetData(cusQuizzesStats);                
            }

        if (tableDisplay != null)
            for (int i = 0; i < tableDisplay.Count; i++)
            {
                tableDisplay[i].GetData(cusQuizzesStats);
            }
        if(questionDropdowns != null)
        {
            for (int i = 0; i < questionDropdowns.Count; i++)
            {
                questionDropdowns[i].GetData(cusQuizzesStats);
            }
        }
        if (quizDropdowns != null)
        {
            for (int i = 0; i < quizDropdowns.Count; i++)
            {
                quizDropdowns[i].GetQuizData(cusQuizzesStats);
            }
        }


    }

    protected override string SetParameterValue(string param)
    {
        throw new System.NotImplementedException();
    }
}
