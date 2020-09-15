using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class SessionOnClick : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] Text infoTextObject = null;
    [SerializeField] Image toggle = null;
    [SerializeField] public SessionList sessionList;

    [Header("Textures")]
    [SerializeField] Sprite uncheckedToggle = null;
    [SerializeField] Sprite checkedToggle = null;
    private int index;
    public void SetIndex(int index)
    {
        this.index = index;
    }
    public int QuizId { get; private set; }
    public void SetQuizId(int id)
    {
        QuizId = id;
    }

    static public string selectedsession;
    private bool Checked = false;
    //[SerializeField] public SceneTransition sceneTransition;
    string currentSceneName;

    public void OnClick(Button btn)
    {
        sessionList.SetSelectedQuizId(this.QuizId , this.index);

        /*currentSceneName = sceneTransition.GetSceneName();
        if (currentSceneName == "ListofsessionScene4")
        {
            sceneTransition.SceneLoader("RoomScene5");
        }
        else if (currentSceneName == "SelectSessionScene15")
        {
            sceneTransition.SceneLoader("AddqnScene10");
        }
        selectedsession = btn.transform.Find("Content").GetComponent<Text>().text;*/
    }

    public void SwitchState(Button btn)
    {
        //sessionList = GameObject.Find("Sessionlist").GetComponent<SessionList>();

        /*if (sessionList.sessionSelected.Count != 0)
        {
            for (int i = 0; i < sessionList.transforms.Count; i++)
            {
                Debug.Log(sessionList.transforms[i].Find("Content").GetComponent<Text>().text);
                if (sessionList.sessionSelected[0] == sessionList.transforms[i].Find("Content").GetComponent<Text>().text)
                {
                    sessionList.transforms[i].Find("Toggle").GetComponent<Image>().sprite = uncheckedToggle;
                }
            }
            sessionList.sessionSelected.Clear();
        }
        if (btn.transform.Find("Toggle").GetComponent<Image>().sprite == uncheckedToggle)
            Checked = false;
        
        sessionList.sessionSelected.Add(btn.transform.Find("Content").GetComponent<Text>().text);
        Checked = !Checked;
        UpdateUI();*/
    }

    void UpdateUI()
    {
        if (toggle == null) return;
        toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
    }
    public void toggleUI(bool check)
    {
        if (toggle == null) return;
        toggle.sprite = (check) ? checkedToggle : uncheckedToggle;
    }
}
