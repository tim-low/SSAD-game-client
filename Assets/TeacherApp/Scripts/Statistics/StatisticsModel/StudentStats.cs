using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{
    [System.Serializable]
    public class StudentStats
    {
        public string name;
        public string classGroup;
        public int year;
        public int semester;
        public int totalAttempts;
        public int totalCorrect;
        public float correctness;

        public StudentStats (string name, string classGroup, int year, int semester, int totalAttempts, int totalCorrect, float correctness)
        {
            this.name = name;
            this.classGroup = classGroup;
            this.year = year;
            this.semester = semester;
            this.totalAttempts = totalAttempts;
            this.totalCorrect = totalCorrect;
            this.correctness = correctness;
        }
    }

    [System.Serializable]
    public class StudentsStats
    {
        public StudentStats[] studentsStats;
    }
}