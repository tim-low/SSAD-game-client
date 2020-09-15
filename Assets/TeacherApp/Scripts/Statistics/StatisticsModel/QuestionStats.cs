using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{
    [System.Serializable]
    public class QuestionStats
    {
        public int questionId;
        public float correctness; //in percentage
        public int correctAnsStudentsCount;
        public int wrongAns1StudentsCount;
        public int wrongAns2StudentsCount;
        public int wrongAns3StudentsCount;
        public string questionText;
        public string correctAnsText;
        public string wrongAns1Text;
        public string wrongAns2Text;
        public string wrongAns3Text;

        public QuestionStats (int qnId, float correctness, int cAnsStudCnt, int wAns1StudCnt, int wAns2StudCnt, int wAns3StudCnt, string correctAnsText, string wrongAns1Text, string wrongAns2Text, string wrongAns3Text)
        {
            this.questionId = qnId;
            this.correctness = correctness;
            this.correctAnsStudentsCount = cAnsStudCnt;
            this.wrongAns1StudentsCount = wAns1StudCnt;
            this.wrongAns2StudentsCount = wAns2StudCnt;
            this.wrongAns3StudentsCount = wAns3StudCnt;
            this.correctAnsText = correctAnsText;
            this.wrongAns1Text = wrongAns1Text;
            this.wrongAns2Text = wrongAns2Text;
            this.wrongAns3Text = wrongAns3Text;
        }
    }

    [System.Serializable]
    public class QuestionsStats
    {
        public QuestionStats[] questionsStats;
    }
}