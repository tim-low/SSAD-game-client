using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace SuperSad.Model
{
    public class Questionlist : MonoBehaviour
    {
        [SerializeField]
        private Transform qncontainer;
        [SerializeField]
        private Transform qntemplate;

        [Header("API")]
        [SerializeField] questionGET questionGET;

        [SerializeField] SceneTransition sceneTransition;
        [SerializeField] Dropdownmanager dropdownManager;


        public List<string> selectedqn = new List<string>() { };
        public List<string> getselectedqns() { return selectedqn; }
        //public List<string> questions;
        public List<string> filterqn = null;
        // List<string> questions = new List<string>() { /*"why", "my life", "so difficult"*/};
        List<string> questions;
        public List<string> getqns() { return questions; }
        private List<Transform> transforms;
        public List<Question> qnapi;
       // public string csn;
        public float templateHeight;

        void Start()
        {
/*            for (int i = 0; i < 5; i++)
            {
                if (api.getqnapi().Count != 0)
                {
                    loadFinish = true;
                    break;
                }
                Debug.Log(api.getqnapi().Count);
            }
            Debug.Log(api.getqnapi().Count);*/
/*            qnapi = api.qnFromapi;
            for (int j = 0; j < qnapi.Count; j++)
            {
                
                questions.Add(qnapi[j].question);
                //Debug.Log(questions[j]);
                Debug.Log(api.qnFromapi[j].question);
            }*/

            //Load(questions);
        }


        public void Load(List<string> qnlist)
        {

            //qncontainer = transform.Find("QuestionContentArea");
            //qntemplate = qncontainer.Find("Questionprefab");

            qntemplate.gameObject.SetActive(false);
            transforms = new List<Transform>() { };


/*            csn = sceneTransition.GetSceneName();
            Debug.Log(csn);*/
/*            if(csn == "CreateNewSessionScene2")
                 templateHeight = 45f;
            else
                templateHeight = 73f;*/
            //float width = qncontainer.GetComponent<Renderer>().bounds.size;


           // RectTransform qnContainerRT = qncontainer.GetComponent<RectTransform>();
            for (int i = 0; i < qnlist.Count; i++)
            {
                Transform entrytransform = Instantiate(qntemplate, qncontainer);

                RectTransform entryRectTransform = entrytransform.GetComponent<RectTransform>();
/*                
                float width = entryRectTransform.GetComponent<RectTransform>().sizeDelta.y;
                Debug.Log(width);*/
                //entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
                entrytransform.gameObject.SetActive(true);


                entrytransform.Find("Content").GetComponent<Text>().text = qnlist[i];
                transforms.Add(entrytransform);

                // expand height of Content
                //qnContainerRT.sizeDelta += new Vector2(0f, entryRectTransform.sizeDelta.y);
            }
        }

        public void Clear()
        {

            if (transforms!=null) {           
                foreach (Transform entry in transforms)
            {

                    Destroy(entry.gameObject);
                }
                transforms.Clear();
            }
        }

        public void Select(string chosen, bool add)
        {
            if (add)
            {
                selectedqn.Add(chosen);
            }
            else if (!add)
            {
                selectedqn.Remove(chosen);
            }
            for (int i = 0; i < selectedqn.Count; i++)
            {
                Debug.Log(selectedqn[i]);
            }
        }

        public void filter()
        {
            
            filterqn = new List<string>() { };
            qnapi = questionGET.qnFromapi;
            questions = new List<string>() { };
            for (int j = 0; j < qnapi.Count; j++)
            {
                questions.Add(qnapi[j].question);
            }

            Clear();

            if (dropdownManager.itemClicked == 0)
            {
                Load(questions);
            }
            else 
            {
                for (int j = 0; j < qnapi.Count; j++)
                {
                    if (qnapi[j].topicId == dropdownManager.itemClicked)
                        filterqn.Add(qnapi[j].question);
                }

                Load(filterqn);
            }

            //Debug.Log(dropdownManager.itemClicked);

           /* else if (opt == 1)
            {
                for (int j = 0; j < qnapi.Count; j++)
                {
                    if (qnapi[j].topicId == 1)
                        filterqn.Add(qnapi[j].question);
                }

                Load(filterqn);
            }
            else if (opt == 2)
            {
                for (int j = 0; j < qnapi.Count; j++)
                {
                    if (qnapi[j].topicId == 2)
                        filterqn.Add(qnapi[j].question);
                }
                Load(filterqn);
            }
            else if (opt == 3)
            {
                for (int j = 0; j < qnapi.Count; j++)
                {
                    if (qnapi[j].topicId == 3)
                        filterqn.Add(qnapi[j].question);
                }
                Load(filterqn);
            }
            else if (opt == 4)
            {
                for (int j = 0; j < qnapi.Count; j++)
                {
                    if (qnapi[j].topicId == 4)
                        filterqn.Add(qnapi[j].question);
                }
                Load(filterqn);
            }
            else if (opt == 5)
            {
                for (int j = 0; j < qnapi.Count; j++)
                {
                    if (qnapi[j].topicId == 5)
                        filterqn.Add(qnapi[j].question);
                }
                Load(filterqn);
            }
            else if (opt == 6)
            {
                for (int j = 0; j < qnapi.Count; j++)
                {
                    if (qnapi[j].topicId == 6)
                        filterqn.Add(qnapi[j].question);
                }
                Load(filterqn);
            }*/

        }


    }
}
