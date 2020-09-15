using Network_Http;
using SuperSad.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class CreateSessionManager : HttpRequestSenderPOST
{
    [Header("UI Elements")]
    [SerializeField] InputField sessionName;
    [SerializeField] InputField noOfQuestions;

    [Header("Topic")]
    [SerializeField] Dropdownmanager dropdownmanager;

    [Header("qnlist")]
    [SerializeField] Questionlist questionlist;
    [SerializeField] questionGET questionGET;

    [SerializeField] Text responseText;
    [SerializeField] GameObject error;
    [SerializeField] Dropdown dropdownObject;
    [SerializeField] PostNewQnsAdded postNewQnsAdded;
    [SerializeField] SceneTransition sceneTransition;


    string json;

    public void AddQuestionToSession()
    {
        bool isValid = true;
        //questionlist = GameObject.Find("Questionlist").GetComponent<Questionlist>();
        //TestAPI testapi = GameObject.Find("Questionlist").GetComponent<TestAPI>();
        int noq = 0;
        List<Question> questions = new List<Question>() { };
        List<int> qnIds = new List<int>() { };
        List<Question> qns = questionGET.qnFromapi;
        string sesid = sessionName.text;
        // Debug.Log(sesid);
        try
        {
            noq = System.Convert.ToInt32(noOfQuestions.text);
        }
        catch (System.FormatException)
        {
            responseText.text = "Invalid number of question";
            isValid = false;
        }

        List<string> qnl = questionlist.getselectedqns();
        if (sesid == "")
        {
            responseText.text = "Please enter a session name.";
            isValid = false;
        }
        else if (noq == 0)
        {
            responseText.text = "Please enter the no of questions.";
            isValid = false;

        }
        else if (noq < 5)
        {
            responseText.text = "Please enter at least 5 questions.";
            isValid = false;
        }
        else if (noq > 30)
        {
            responseText.text = "Please enter less than 30 questions.";
            isValid = false;
        }
        else if (qnl.Count != noq)
        {
            responseText.text = "No. of questions not equal to No. of questions selected.";
            isValid = false;
        }
        else
        {
            responseText.text = "Session " + sesid + " created";

            //Debug.Log(qns.Count);
            for (int i = 0; i < qnl.Count; i++)
            {
                for (int j = 0; j < qns.Count; j++)
                {
                    if (qns[j].question == qnl[i])
                    {
                        questions.Add(qns[j]);
                    }

                }
                // Debug.Log(qnl[i]);
            }
            for (int i = 0; i < questions.Count; i++)
            {
                qnIds.Add(questions[i].id);
            }
            CustomQuiz customQuiz = new CustomQuiz(sesid, qnIds);
            // ListContainer container = new ListContainer(customQuiz);
            json = JsonUtility.ToJson(customQuiz);
            Debug.Log(json);
        }
        if (isValid)
        {
            responseText.text = "Reading ...";
            SendHttpRequest();
        }
        error.SetActive(true);
    }

    public void ReloadScene()
    {
        if (!postNewQnsAdded.btnIsClosingForm) //button is closing form
        {
            sceneTransition.SceneLoader("CreateNewQuizScene2");
        }
    }

    public void ClearInputs()
    {
        sessionName.text = "";
        noOfQuestions.text = "";
        dropdownObject.value = 0;
    }

    protected override WWWForm CreateRequestBody()
    {
        throw new System.NotImplementedException();
    }

    protected override string CreateRequestRaw()
    {
        //Debug.Log(responseText.text);
        Debug.Log(json);
        return json;
    }

    protected override string GetRequestUrl()
    {
        Debug.Log("http://134.209.98.43:8000/api/v1/quiz/post/" + UserState.Instance().Token);
        return "http://134.209.98.43:8000/api/v1/quiz/post/" + UserState.Instance().Token;
    }

    protected override void OnBadRequestError(UnityWebRequest www)
    {
        // Perform actions on unsuccessful response - request is not correct
        responseText.gameObject.SetActive(true);
        responseText.text = "Bad Request";
        error.SetActive(true);
    }

    protected override void OnInternalServerError(UnityWebRequest www)
    {
        responseText.gameObject.SetActive(true);
        responseText.text = "Internal Server Error";
        error.SetActive(true);
    }

    protected override void OnRequestSuccess(UnityWebRequest www)
    {
        // Perform actions on successful response
        responseText.gameObject.SetActive(true);

        ServerResponse response = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
        //Debug.Log(www.downloadHandler.text);
        //Debug.Log(response.responseMsg);
        responseText.text = response.responseMsg;
        Debug.Log(responseText.text);
        error.SetActive(true);
        ClearInputs();
    }
}
