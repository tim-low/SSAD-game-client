using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SuperSad.Model;
using UnityEngine.UI;
using System.Collections.Generic;

public class questionGET : MonoBehaviour
{
    public Questions questionsObj;
    public Question questionObj;
    [Header("qnlist")]
    [SerializeField] Questionlist questionlist;
    public List<Question> qnFromapi = new List<Question>() { };
    public List<Question> getqnapi() { return qnFromapi; }
    List<string> questions = new List<string>() { };

    void Start()
    {
        StartCoroutine(GetText());
        Debug.Log("http://134.209.98.43:8000/api/v1/question/get/" + UserState.Instance().Token);
    }

    IEnumerator GetText()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://134.209.98.43:8000/api/v1/question/get/" + UserState.Instance().Token))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);
                questionsObj = JsonUtility.FromJson<Questions>(www.downloadHandler.text);
                for (int i = 0; i < questionsObj.questions.Length; i++)
                {
                    questionObj = questionsObj.questions[i];
                    Question qn = new Question(questionObj.id, questionObj.question, questionObj.topicId);
                    qnFromapi.Add(qn);
                    //Debug.Log(qnFromapi[i].id);
                }
                    
                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;

                for (int j = 0; j < qnFromapi.Count; j++)
                {

                    questions.Add(qnFromapi[j].question);
                }
                //questionlist.Load(questions);
                //questionlist = GameObject.Find("Questionlist").GetComponent<Questionlist>();
                questionlist.Load(questions);
            }
        }
    }
}
