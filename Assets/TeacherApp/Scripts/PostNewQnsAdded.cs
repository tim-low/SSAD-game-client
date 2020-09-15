using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Network_Http;
using SuperSad.Model;

public class PostNewQnsAdded : HttpRequestSenderPOST
{
    [Header("UI Elements")]
    [SerializeField] InputField question;
    [SerializeField] InputField correctAnswer;
    [SerializeField] InputField wrongAnswer1;
    [SerializeField] InputField wrongAnswer2;
    [SerializeField] InputField wrongAnswer3;
    [SerializeField] Text responseText;
    [SerializeField] GameObject error;

    [SerializeField] GameObject NewQnForm;

    [Header("Topic")]
    [SerializeField] Dropdownmanager dropdownmanager;

    [SerializeField] Dropdown dropdownObject;

    public bool btnIsClosingForm = false;

/*    [Header("POST")]
    [SerializeField] Postdata postdata;*/

    string json;
    
    [SerializeField] SceneTransition sceneTransition;

    public bool Read() //if server is down, execute this only to see if the UI is correct
    {
        bool isValid = true;
        string errorMsg = "";

        List<Answers> answers = new List<Answers>() { };
        
        if (dropdownmanager.itemClicked == 0)
        {
            errorMsg += "Please input a Topic.\n";
            isValid = false;
        }
        if (question.text == "")
        {
            errorMsg += "Please input a Question.\n";
            isValid = false;
        }
        else if (question.text.Length < 1 || correctAnswer.text.Length > 1024)
        {
            errorMsg += "The question statement must be at least 1 character and less than 1024 characters.\n";
            isValid = false;
        }
        if (correctAnswer.text == "")
        {
            errorMsg += "Please input a Correct answer.\n";
            isValid = false;
        }
        else if (correctAnswer.text.Length < 1 || correctAnswer.text.Length > 256)
        {
            errorMsg += "Correct Answer: An answer must be at least 1 character and less than 256 characters.\n";
            isValid = false;
        }
        if (wrongAnswer1.text == "")
        {
            errorMsg += "Please input a Wrong answer 1.\n";
            isValid = false;
        }
        else if (wrongAnswer1.text.Length < 1 || wrongAnswer1.text.Length > 256)
        {
            errorMsg += "Wrong Answer 1: An answer must be at least 1 character and less than 256 characters.\n";
            isValid = false; 
        }
        if (wrongAnswer2.text == "")
        {
            errorMsg += "Please input a Wrong answer 2.\n";
            isValid = false;
        }
        else if (wrongAnswer2.text.Length < 1 || wrongAnswer2.text.Length > 256)
        {
            errorMsg += "Wrong Answer 2: An answer must be at least 1 character and less than 256 characters.\n";
            isValid = false;
        }
        if (wrongAnswer3.text == "")
        {
            errorMsg += "Please input a Wrong answer 3.\n";
            isValid = false;
        }
        else if (wrongAnswer3.text.Length < 1 || wrongAnswer3.text.Length > 256)
        {
            errorMsg += "Wrong Answer 3: An answer must be at least 1 character and less than 256 characters.\n";
            isValid = false;
        }
        if (isValid == true)
        {
            Debug.Log(wrongAnswer1.text.Length);
            string[] wrongAnsArr = new string[3];

            wrongAnsArr[0] = wrongAnswer1.text;
            wrongAnsArr[1] = wrongAnswer2.text;
            wrongAnsArr[2] = wrongAnswer3.text;

            Question newQn = new Question(question.text, dropdownmanager.itemClicked, correctAnswer.text, wrongAnsArr);
            json = JsonUtility.ToJson(newQn);
            Debug.Log(json);
            //errorMsg = "Successfully created question!";
        }
        responseText.text = errorMsg;
        error.SetActive(true); //just to test since server is down
        return isValid;
    }

    bool changeSceneOK;

    public void SendRequest()
    {
        if (Read())
        {
            responseText.text = "Reading ...";
            SendHttpRequest();
        }
    }

    public void ChangeScene()
    {
        if (changeSceneOK)
        {
            sceneTransition.ConditionalSceneLoader();
        }
        //if (responseText.text == "Internal Server Error")
        //{
        //    sceneTransition.ConditionalSceneLoader();
        //}
    }

    public void CloseForm()
    {
        if (NewQnForm.activeSelf)
        {
            if (changeSceneOK)
            {
                NewQnForm.SetActive(false);
                btnIsClosingForm = true;
            }
        }
    }

    public void ClearInputs()
    {
        dropdownObject.value = 0;
        question.text = "";
        correctAnswer.text = "";
        wrongAnswer1.text = "";
        wrongAnswer2.text = "";
        wrongAnswer3.text = "";
    }

    protected override WWWForm CreateRequestBody()
    {
        throw new System.NotImplementedException();
    }

    protected override string CreateRequestRaw()
    {
        Debug.Log("hello?");
        Debug.Log(responseText.text);
        Debug.Log(json);
        return json;
    }

    protected override string GetRequestUrl()
    {
        return "http://134.209.98.43:8000/api/v1/question/post/"+UserState.Instance().Token;    // append the token
    }

    protected override void OnRequestSuccess(UnityWebRequest www)
    {
        // Perform actions on successful response
        responseText.gameObject.SetActive(true);

        //Debug.Log(www.downloadHandler.text);
        ServerResponse response = JsonUtility.FromJson<ServerResponse>(www.downloadHandler.text);
        responseText.text = response.responseMsg;
        //if (responseText.text == "Successfully created question!")
        //{
        //    responseText.text = "Success";
        //}
        error.SetActive(true);
        changeSceneOK = true;
        ClearInputs();
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
        // Perform actions on unsuccessful response - internal server error
        responseText.gameObject.SetActive(true);
        responseText.text = "Internal Server Error";
        error.SetActive(true);
    }
}
