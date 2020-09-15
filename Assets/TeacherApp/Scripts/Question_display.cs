using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperSad.Model {
    public class Question_display : MonoBehaviour
    {


        #region Variables
        [Header("UI Elements")]
        [SerializeField] Text infoTextObject = null;
        [SerializeField] Image toggle = null;

        [Header("Textures")]
        [SerializeField] Sprite uncheckedToggle = null;
        [SerializeField] Sprite checkedToggle = null;


        [Header("Onclick")]
        [SerializeField] Questionlist questionlist = null;

        private RectTransform _rect = null;
        public RectTransform Rect
        {
            get
            {
                if (_rect == null)
                {
                    _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
                }
                return _rect;
            }
        }

        private int _qnIndex = -1;
        public int QnIndex { get { return _qnIndex; } }
        private bool Checked = false;
        public string content;
        public Button ThisButton;
        List<string> setToggle = new List<string>() { };
        #endregion


        void Start()
        {
            setToggle = questionlist.selectedqn;
            for (int i = 0; i < setToggle.Count; i++)
            {
                if (transform.Find("Content").GetComponent<Text>().text == setToggle[i])
                {
                    Checked = true;
                    UpdateUI();
                }

            }
        }

        public void UpdateData(string info, int index)
        {
            content = info;
            _qnIndex = index;

        }

        public void Reset()
        {
            Checked = false;
            UpdateUI();
        }


        public void SwitchState(Button btn)
        {
            string chosen = btn.transform.Find("Content").GetComponent<Text>().text;
            Checked = !Checked;
            questionlist.Select(chosen, Checked);
            UpdateUI();

        }



        void UpdateUI()
        {
            if (toggle == null) return;
            toggle.sprite = (Checked) ? checkedToggle : uncheckedToggle;
        }
    }
}
