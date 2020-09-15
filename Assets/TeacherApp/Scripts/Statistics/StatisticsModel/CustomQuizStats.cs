using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{
    [System.Serializable]
    public class CustomQuizStats
    {
        public string quizName;
        public int highestScore; //highestScore that a student obtained
        public int lowestScore;
        public float mean;
        public float median;
        public float standardDeviation;
        public QuestionStats[] questionStats; //eh json got typo supposed to be questionsStats
        public Student[] studentsAttempted;

        public CustomQuizStats() { }

        public CustomQuizStats(string quizName, int highestScore, int lowestScore, float mean, float median, float standardDeviation, QuestionStats[] qnsStats , Student[] studsAttempted)
        {
            this.quizName = quizName;
            this.highestScore = highestScore;
            this.lowestScore = lowestScore;
            this.mean = mean;
            this.median = median;
            this.standardDeviation = standardDeviation;
            this.questionStats = qnsStats;
            this.studentsAttempted = studsAttempted;
        }
    }

    [System.Serializable]
    public class CustomQuizzesStats
    {
        public CustomQuizStats[] quizStat;
    }
}