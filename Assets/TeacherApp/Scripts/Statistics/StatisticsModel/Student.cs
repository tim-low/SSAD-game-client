using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{
    [System.Serializable]
    public class Student
    {
        public string studentName;
        public string classGroup;
        public int year;
        //public int semester; //not inside yet
        public int[] answerPicked;
        public int numCorrect;
        public float correctness; // in percentage

        public Student(string studName, string classGrp, int year, int[] ansPicked, int numCorrect, float correctness)
        {
            this.studentName = studName;
            this.classGroup = classGrp;
            this.year = year;
            this.answerPicked = ansPicked;
            this.numCorrect = numCorrect;
            this.correctness = correctness;
        }
    }

    [System.Serializable]
    public class Students
    {
        public Student[] studentsStats;
    }
}