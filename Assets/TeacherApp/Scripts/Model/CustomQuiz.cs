using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{
    [System.Serializable]
    public class CustomQuiz
    {
        public int Id;
        public string quizName;
        public int IsCustom;
        public string CreatedBy;
        public int numQuestions;
        public string Sessions;
        public List<int> questionIds;

        public CustomQuiz() { }

        public CustomQuiz(string quizName,List<int> questionIds)
        {
            this.quizName = quizName;
            this.questionIds = questionIds;
        }

        public CustomQuiz(int Id, string quizName, int IsCustom, string CreatedBy, int numQuestions, string Sessions)
        {
            this.Id = Id;
            this.quizName = quizName;
            this.IsCustom = IsCustom;
            this.CreatedBy = CreatedBy;
            this.numQuestions = numQuestions;
            this.Sessions = Sessions;
        }
    }

    [System.Serializable]
    public class CustomQuizzes
    {
        public CustomQuiz[] quiz;
    }
}