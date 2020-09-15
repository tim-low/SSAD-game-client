using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SuperSad.Model;

namespace SuperSad.Gameplay
{
    public class AnswerButton : MonoBehaviour {

        [SerializeField]
        private Text answerText;
        [SerializeField]
        private Image buttonImage;
        [SerializeField]
        private Color selectedColor;
        private Color defaultColor;

        public Answer answer { get; private set; }
        public int AnswerId
        {
            get { return answer.Id; }
        }

        void Awake()
        {
            // Get default Image Color
            defaultColor = buttonImage.color;
        }

        public void SetAnswer(Answer answer)
        {
            this.answer = answer;
            answerText.text = answer.Text;
        }

        public void SetSelectedColor()
        {
            buttonImage.color = selectedColor;
        }
        public void SetDefaultColor()
        {
            buttonImage.color = defaultColor;
        }
    }
}