using Network_Http;
using SuperSad.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SessionList : HttpRequestSenderGET
{

    [Header("Input")]
    [SerializeField] InputField noOfRooms;
    [SerializeField]
    private Transform sscontainer;
    [SerializeField]
    private Transform sstemplate;
    [SerializeField]
    private GameObject hostButton;
    List<string> Sessions = new List<string>() { "This", "is", "sessionlist" };
    public List<Transform> transforms;
    public int SelectedQuizId { get; private set; }    // id of selected quiz
    private List<SessionOnClick> sessionOnClicks = new List<SessionOnClick>() { };
    private int selectSessionIndex = 0;
    //private Text responseText;
    bool success = false;
    CustomQuizzes customQuizzes;
    GameObject customQuizObj;
    [SerializeField] GameObject customQuizPrefab;
    [SerializeField] GameObject verticalLayoutGroup;
    //[SerializeField] SceneTransition sceneTransition;

    [SerializeField]
    [Tooltip("For error messages")]
    private Text displayText;

    // Use this for initialization
    void Start () {
        //Load(Sessions);
        SendHttpRequest();
        //if (success)
        //{
        //    LoadSessions();
        //}

        displayText.gameObject.SetActive(false);
    }

    //public void Load(List<string> sslist)
    //{

    //    sscontainer = transform.Find("SessionContentArea");
    //    sstemplate = sscontainer.Find("Session");

    //    sstemplate.gameObject.SetActive(false);

    //    float templateHeight = 78f;

    //    for (int i = 0; i < sslist.Count; i++)
    //    {
    //        Transform entrytransform = Instantiate(sstemplate, sscontainer);

    //        RectTransform entryRectTransform = entrytransform.GetComponent<RectTransform>();
    //        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
    //        entrytransform.gameObject.SetActive(true);
    //        entrytransform.Find("Content").GetComponent<Text>().text = sslist[i];
    //        transforms.Add(entrytransform);

    //    }

    //}

    public void LoadSessions()
    {
        Debug.Log(customQuizzes.quiz.Length);
        if (customQuizzes.quiz.Length == 0) // no quizzes
        {
            displayText.text = "You have no quizzes!";
            displayText.gameObject.SetActive(true);
        }
        else
        {
            ;
            for (int i = 0; i < customQuizzes.quiz.Length; i++)
            {
                customQuizObj = Instantiate(customQuizPrefab);
                customQuizObj.transform.SetParent(verticalLayoutGroup.transform, false);
                customQuizObj.transform.GetChild(1).GetComponent<Text>().text = customQuizzes.quiz[i].quizName;
                //customQuizObj.GetComponent<SessionOnClick>().sceneTransition = sceneTransition;
                SessionOnClick tempSessionClick = customQuizObj.GetComponent<SessionOnClick>();
                sessionOnClicks.Add(tempSessionClick);
                tempSessionClick.sessionList = this;
                tempSessionClick.SetQuizId(customQuizzes.quiz[i].Id);
                tempSessionClick.SetIndex(i);
                Debug.Log(customQuizObj.transform.GetChild(1).GetComponent<Text>().text);
            }
        }
    }

    public void SetSelectedQuizId(int id , int index)
    {
        sessionOnClicks[selectSessionIndex].toggleUI(false);
        sessionOnClicks[index].toggleUI(true);
        selectSessionIndex = index;
        SelectedQuizId = id;
        hostButton.SetActive(true);
    }

    protected override string SetParameterValue(string param)
    {
        throw new System.NotImplementedException();
    }

    protected override string GetRequestUrl()
    {
        return "http://134.209.98.43:8000/api/v1/quiz/get/" + UserState.Instance().Token; //appended with token
    }

    protected override void OnBadRequestError(UnityWebRequest www)
    {
        // Perform actions on unsuccessful response - request is not correct
        displayText.gameObject.SetActive(true);
        displayText.text = "Bad Request";
    }

    protected override void OnInternalServerError(UnityWebRequest www)
    {
        // Perform actions on unsuccessful response - internal server error
        displayText.gameObject.SetActive(true);
        displayText.text = "Internal Server Error";
    }

    protected override void OnRequestSuccess(UnityWebRequest www)
    {
        // Perform actions on successful response
        //responseText.gameObject.SetActive(true);
        //responseText.text = "Success";
        success = true;
        customQuizzes = JsonUtility.FromJson<CustomQuizzes>(www.downloadHandler.text);
        Debug.Log(www.downloadHandler.text);
        LoadSessions();
    }
}
