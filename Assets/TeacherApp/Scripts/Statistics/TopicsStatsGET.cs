using Network_Http;
using SuperSad.Model;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using SuperSad.Model;
using System.Collections.Generic;

public class TopicsStatsGET : HttpRequestSenderGET
{
    [SerializeField] Text responseText;
    private bool GETsuccess = false;
    private string topicsStatsJson;
    public TopicsMasteryStats topicsMasteryStats;
    [SerializeField] List<TableDisplay> tableDisplay;
    [SerializeField] List<GraphDisplay> graphDisplay;

    [SerializeField] List<Dropdownmanager> classDropdownManager;
    [SerializeField] List<Dropdownmanager> yearDropdownManager;
    [SerializeField] List<Dropdownmanager> semDropdownManager;
    [SerializeField] List<Dropdownmanager> classPerfYearDropdownManager;



    void Start()
    {
        LoadTopicsStats();
    }
    public void LoadTopicsStats()
    {
        SendHttpRequest();
        if (GETsuccess)
        {
            Debug.Log(topicsStatsJson);
            topicsMasteryStats = JsonUtility.FromJson<TopicsMasteryStats>(topicsStatsJson);
            //display Data
        }
    }

    protected override string GetRequestUrl()
    {
        return "http://134.209.98.43:8000/api/v1/analytic/topic/" + UserState.Instance().Token;
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
        topicsStatsJson = www.downloadHandler.text;
        //Debug.Log(www.downloadHandler.text);
        topicsMasteryStats = JsonUtility.FromJson<TopicsMasteryStats>(topicsStatsJson);
        Debug.Log(topicsMasteryStats.topicMasteryStats.Length);

        if (classDropdownManager != null)
        {
            for (int i = 0; i < classDropdownManager.Count; i++)
            {
                classDropdownManager[i].GetClassData(topicsMasteryStats);
            }
        }
        if (yearDropdownManager != null)
        {
            for (int i = 0; i < yearDropdownManager.Count; i++)
            {
                yearDropdownManager[i].GetYearData(topicsMasteryStats);
            }
        }

        if (classPerfYearDropdownManager != null)
        {
            for (int i = 0; i < yearDropdownManager.Count; i++)
            {
                classPerfYearDropdownManager[i].GetClassYearData(topicsMasteryStats);
            }
        }
        if (semDropdownManager != null)
        {
            for (int i = 0; i < semDropdownManager.Count; i++)
            {
                semDropdownManager[i].GetSemData(topicsMasteryStats);
            }
        }
        if (graphDisplay != null)
            for (int i = 0; i < graphDisplay.Count; i++)
            {
                graphDisplay[i].GetStudData(topicsMasteryStats);
            }

        if (tableDisplay != null)
            for (int i = 0; i < tableDisplay.Count; i++)
            {
                tableDisplay[i].GetStudData(topicsMasteryStats);
            }





    }

    protected override string SetParameterValue(string param)
    {
        throw new System.NotImplementedException();
    }
}
