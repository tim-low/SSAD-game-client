using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SuperSad.Model;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class QnsDisplay : MonoBehaviour
{
    public Questions questionsObj;
    public Question questionObj;
    public GameObject myPrefab;
    public GameObject verticalLayoutGroupObj;
    Toggle toggle;

    //private List<Toggle> checkboxes;
    //public int TogglesOn { get; private set; }
    //public int TogglesOff { get; private set; }


    void Start()
    {
        StartCoroutine(GetText());

        //checkboxes = GetComponentsInChildren<Toggle>().ToList();

        //foreach (Toggle toggle in checkboxes)
        //{
        //    // get an initial count of on/off
        //    if (toggle.isOn)
        //    {
        //        TogglesOn++;
        //    }
        //    else
        //    {
        //        TogglesOff++;
        //    }

        //    // listen for changes
        //    toggle.onValueChanged.AddListener(OnToggleValueChanged);
        //}
    }

    string actual = "http://134.209.98.43:8000/api/v1/question/get/" + UserState.Instance().Token;

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(actual))
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
    }

    private void OnToggleValueChanged(bool isOn)
    {
        //TogglesOn += isOn ? 1 : -1;
        //TogglesOff += !isOn ? 1 : -1;

        //print("Toggles On: " + TogglesOn);
        //print("Toggles Off: " + TogglesOff);
        //print("Checks: " + TotalToggles);

        if (isOn)
        {
            
        }
    }

    //string test = "https://s5.aconvert.com/convert/p3r68-cdx67/uh00z-pnzjh.json";

    //IEnumerator GetText()
    //{
    //    using (UnityWebRequest www = UnityWebRequest.Get(test))
    //    {
    //        yield return www.Send();

    //        if (www.isNetworkError || www.isHttpError)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            GameObject qnsWCheckBox;
    //            GameObject toggle;
    //            GameObject label;

    //            // Show results as text
    //            Debug.Log(www.downloadHandler.text);

    //            //questionsObj = JsonUtility.FromJson<Questions>(www.downloadHandler.text);

    //            for (int i = 0; i < 44; i++)
    //            {
    //                //questionObj = questionsObj.questions[0];
    //                questionObj = JsonUtility.FromJson<Question>(www.downloadHandler.text);
    //                Debug.Log(questionObj.question);
    //                qnsWCheckBox = Instantiate(myPrefab);
    //                qnsWCheckBox.transform.SetParent(verticalLayoutGroupObj.transform);
    //                toggle = qnsWCheckBox.transform.GetChild(0).gameObject;
    //                toggle.SetActive(true);
    //                label = qnsWCheckBox.transform.GetChild(0).GetChild(1).gameObject;
    //                Debug.Log(label.name);
    //                label.GetComponent<Text>().text = questionObj.question;
    //            }

    //            // Or retrieve results as binary data
    //            byte[] results = www.downloadHandler.data;
    //        }
    //    }
    //}

}