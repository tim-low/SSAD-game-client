using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{

    [System.Serializable]
    public class Question
    {
        public int id;
        public string question;
        public int topicId;
        public string correctAns;
        public string[] wrongAns;

        public Question() { }

        public Question(string question, int topicId)
        {
            this.question = question;
            this.topicId = topicId;
        }

        public Question (string question, int topicId, string correctAns, string[] wrongAns)
        {
            this.question = question;
            this.topicId = topicId;
            this.correctAns = correctAns;
            this.wrongAns = wrongAns;
        }

        public Question(int id, string question, int topicId)
        {
            this.id = id;
            this.question = question;
            this.topicId = topicId;
        }

    }

    [System.Serializable]
    public class Questions
    {
        public Question[] questions;
    }
}