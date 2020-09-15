using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{
    [System.Serializable]
    public class Answers
    {
        public int Id;
        public string Text;

        public Answers() { }

        public Answers( int answerId, string text)
        {
            this.Id = answerId;
            this.Text = text;
        }
    }
}