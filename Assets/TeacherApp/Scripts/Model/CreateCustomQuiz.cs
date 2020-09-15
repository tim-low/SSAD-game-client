using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{
    [System.Serializable]
    public class CreateCustomQuiz
    {
        public string username;
        public string sessionId;
        public List<int> questionId;
        public int noOfQuestions;

        public CreateCustomQuiz(string username, string sessionId, List<int> questionId, int noOfQuestions)
        {
            this.username = username;
            this.sessionId = sessionId;
            this.questionId = questionId;
            this.noOfQuestions = noOfQuestions;
        }
    }
}