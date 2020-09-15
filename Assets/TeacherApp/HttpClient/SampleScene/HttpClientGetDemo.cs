using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network_Http;
using UnityEngine.Networking;
using UnityEngine.UI;
using SuperSad.Model;

public class HttpClientGetDemo : HttpRequestSenderGET {

    [SerializeField]
    private string token;
    [SerializeField]
    private Text responseText;


    public Questions questionsObj;
    public Question questionObj;
    public GameObject myPrefab;
    public GameObject verticalLayoutGroupObj;
    Toggle toggle;

    /*
    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(GetRequestUrl()))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {

                GameObject qnsWCheckBoxObj;
                GameObject toggleObj;
                GameObject labelObj;

                // Show results as text
                Debug.Log(www.downloadHandler.text);

                //putting the all the questions object from the json into the gameobject
                questionsObj = JsonUtility.FromJson<Questions>(www.downloadHandler.text);

                //one by one adding each question from json into a question object
                for (int i = 0; i < questionsObj.questions.Length; i++)
                {
                    questionObj = questionsObj.questions[i];
                    //Debug.Log(questionObj.question);
                    qnsWCheckBoxObj = Instantiate(myPrefab); //the "item" in the list with checkbox
                    qnsWCheckBoxObj.transform.SetParent(verticalLayoutGroupObj.transform);
                    toggleObj = qnsWCheckBoxObj.transform.GetChild(0).gameObject; //checkbox
                    labelObj = qnsWCheckBoxObj.transform.GetChild(0).GetChild(1).gameObject; //label of the item
                    //Debug.Log(label.name);

                    //setting the name of the label to the question string
                    labelObj.GetComponent<Text>().text = questionObj.question;
                    
                }

  
                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }*/

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

    protected override string SetParameterValue(string param)
    {
        switch (param)
        {
            case "examplekey":
                return token;
        }

        return "";
    }

    protected override string GetRequestUrl()
    {
        return "http://134.209.98.43:8000/api/v1/question/get"; // append token here
    }
}
