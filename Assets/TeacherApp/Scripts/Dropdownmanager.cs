using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace SuperSad.Model
{
    public class Dropdownmanager : MonoBehaviour
    {

        [SerializeField] Dropdown dropdown;
        [SerializeField] Questionlist questionlist;
        [SerializeField] GraphDisplay graphDisplay;
        [SerializeField] TableDisplay tableDisplay;
        public int itemClicked;
        List<string> questions;
        CustomQuizzesStats cusQuizzesStats;
        TopicsMasteryStats topicsMasteryStats;
        List<string> quizzes;
        List<List<StudentStats>> byClass;
        List<StudentStats> subByClass;


        // Update is called once per frame
        public void click()
        {
            setItemClicked();
            questionlist.filter();
        }


        public void graphChangeTopic()
        {
            setItemClicked();
            graphDisplay.reloadTopicData();
        }



        public void graphChangeStats()
        {
            setItemClicked();
            graphDisplay.reloadStatsData();

        }

        public void graphChangeClass()
        {
            setItemClicked();
            graphDisplay.reloadClassData();
        }

        public void graphChangeYear()
        {
            setItemClicked();
            graphDisplay.reloadYearData();
        }

        public void graphChangeSem()
        {
            setItemClicked();
            graphDisplay.reloadSemData();
        }

        public void graphChangeClassYear()
        {
            setItemClicked();
            graphDisplay.reloadClassYearData();
        }

        public void graphChangeQuiz()
        {
            setItemClicked();
            graphDisplay.reloadQuizData();
        }
        public void graphChangeQuestion()
        {
            setItemClicked();
            graphDisplay.reloadQuestionData();
        }


        public void TableChangeTopic()
        {
            setItemClicked();
            tableDisplay.reloadTopicData();
        }



        public void TableChangeClassYear()
        {
            setItemClicked();
            tableDisplay.reloadClassYearData();
        }

        public void TableChangeClass()
        {
            setItemClicked();
            tableDisplay.reloadClassData();
        }



        public void TableChangeQuiz()
        {
            setItemClicked();
            tableDisplay.reloadQuizData();
        }


        public void TableChangeYear()
        {
            setItemClicked();
            tableDisplay.reloadYearData();
        }

        public void TableChangeSem()
        {
            setItemClicked();
            tableDisplay.reloadSemData();
        }

        public void CreateQn()
        {
            setItemClicked();
        }

        public void setItemClicked()
        {
            switch (dropdown.value)
            {
                case 0:
                    itemClicked = 0;
                    break;
                case 1:
                    itemClicked = 1;
                    break;
                case 2:
                    itemClicked = 2;
                    break;
                case 3:
                    itemClicked = 3;
                    break;
                case 4:
                    itemClicked = 4;
                    break;
                case 5:
                    itemClicked = 5;
                    break;
                case 6:
                    itemClicked = 6;
                    break;
                case 7:
                    itemClicked = 7;
                    break;
            }
        }



        public void GetData(CustomQuizzesStats cusQuizzesStats)
        {
            this.cusQuizzesStats = cusQuizzesStats;
            string quiz = cusQuizzesStats.quizStat[0].quizName;
            populateQuestionDropdown(quiz);
        }


        public void populateQuestionDropdown(string quiz)
        {

            questions = new List<string>() { };
            for (int i = 0; i < cusQuizzesStats.quizStat.Length; i++)
            {

                if (cusQuizzesStats.quizStat[i].quizName == quiz)
                {
                    for (int j = 0; j < cusQuizzesStats.quizStat[i].questionStats.Length; j++)
                    {
                        questions.Add("Question " + cusQuizzesStats.quizStat[i].questionStats[j].questionId.ToString());

                    }
                }
            }

            dropdown.options.Clear();
            dropdown.AddOptions(questions);
        }



        public void GetQuizData(CustomQuizzesStats cusQuizzesStats)
        {
            this.cusQuizzesStats = cusQuizzesStats;
            populateQuizDropdown();
        }


        public void populateQuizDropdown()
        {
            quizzes = new List<string>() { };
            for (int i = 0; i < cusQuizzesStats.quizStat.Length; i++)
            {

                quizzes.Add(cusQuizzesStats.quizStat[i].quizName);
            }
            dropdown.options.Clear();
            dropdown.AddOptions(quizzes);
        }

        public void GetClassData(TopicsMasteryStats topicsMasteryStats)
        {
            this.topicsMasteryStats = topicsMasteryStats;
            string year = topicsMasteryStats.topicMasteryStats[0].studentsStats[0].year.ToString();
            FilterByClass("1", "-1");
        }




        public void FilterByClass(string topic, string year)
        {
            List<StudentStats> studentStats = new List<StudentStats>() { };
            for (int i = 0; i < topicsMasteryStats.topicMasteryStats.Length; i++)
            {
                if (topicsMasteryStats.topicMasteryStats[i].topicId.ToString() == topic)
                {
                    for (int j = 0; j < topicsMasteryStats.topicMasteryStats[i].studentsStats.Length; j++)
                    {

                        studentStats.Add(new StudentStats(topicsMasteryStats.topicMasteryStats[i].studentsStats[j].name, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].classGroup
                            , topicsMasteryStats.topicMasteryStats[i].studentsStats[j].year, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].semester, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalAttempts
                            , topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalCorrect, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].correctness));
                    }
                }

            }
            List<StudentStats> temp = new List<StudentStats>() { };

            if (year != "-1")
            {
                for (int i = 0; i < studentStats.Count; i++)
                {
                    if (studentStats[i].year.ToString() == year)
                    {
                        temp.Add(studentStats[i]);
                    }
                }
                studentStats = temp;
            }


            byClass = new List<List<StudentStats>>() { };
            subByClass = new List<StudentStats>() { };
            subByClass.Add(studentStats[0]);
            byClass.Add(subByClass);
            for (int i = 1; i < studentStats.Count; i++)
            {

                for (int j = 0; j < byClass.Count; j++)
                {
                    if (studentStats[i].classGroup == byClass[j][0].classGroup)
                    {
                        byClass[j].Add(studentStats[i]);
                        break;
                    }
                    if (j == byClass.Count - 1)
                    {
                        List<StudentStats> newSubByClass = new List<StudentStats>() { };
                        newSubByClass.Add(studentStats[i]);
                        byClass.Add(newSubByClass);
                        break;
                    }
                }

            }
            List<string> classes = new List<string>() { };
            for (int i = 0; i < byClass.Count; i++)
            {
                classes.Add(byClass[i][0].classGroup);
            }
            dropdown.options.Clear();
            dropdown.AddOptions(classes);
            itemClicked = 0;

        }


        public void GetYearData(TopicsMasteryStats topicsMasteryStats)
        {
            this.topicsMasteryStats = topicsMasteryStats;
            string classGroup = topicsMasteryStats.topicMasteryStats[0].studentsStats[0].classGroup;
            FilterByYear("1", classGroup);


        }

        public void GetClassYearData(TopicsMasteryStats topicsMasteryStats)
        {
            this.topicsMasteryStats = topicsMasteryStats;

            FilterByYear("1", "-1");
        }

        public void FilterByYear(string topic, string classGroup)
        {

            List<StudentStats> studentStats = new List<StudentStats>() { };
            for (int i = 0; i < topicsMasteryStats.topicMasteryStats.Length; i++)
            {
                if (topicsMasteryStats.topicMasteryStats[i].topicId.ToString() == topic)
                {
                    for (int j = 0; j < topicsMasteryStats.topicMasteryStats[i].studentsStats.Length; j++)
                    {

                        studentStats.Add(new StudentStats(topicsMasteryStats.topicMasteryStats[i].studentsStats[j].name, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].classGroup
                            , topicsMasteryStats.topicMasteryStats[i].studentsStats[j].year, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].semester, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalAttempts
                            , topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalCorrect, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].correctness));
                    }
                }

            }

            List<string> year = new List<string>() { };
            List<StudentStats> temp = new List<StudentStats>() { };
            if (classGroup != "-1")
            {
                for (int i = 0; i < studentStats.Count; i++)
                {
                    if (studentStats[i].classGroup.ToString() == classGroup)
                    {
                        temp.Add(studentStats[i]);
                    }
                }
                studentStats = temp;
            }

            for (int i = 0; i < studentStats.Count; i++)
            {
                year.Add(studentStats[i].year.ToString());


            }

            List<string> newYear = year.Distinct().ToList();
            dropdown.options.Clear();
            dropdown.AddOptions(newYear);
            itemClicked = 0;

        }



        public void GetSemData(TopicsMasteryStats topicsMasteryStats)
        {
            this.topicsMasteryStats = topicsMasteryStats;
            string year = topicsMasteryStats.topicMasteryStats[0].studentsStats[0].year.ToString();
            string classgrp = topicsMasteryStats.topicMasteryStats[0].studentsStats[0].classGroup.ToString();
            FilterSemByYear("1", classgrp, year);
        }



        public void FilterSemByYear(string topic, string classgrp, string year)
        {
            List<StudentStats> studentStats = new List<StudentStats>() { };
            for (int i = 0; i < topicsMasteryStats.topicMasteryStats.Length; i++)
            {
                if (topicsMasteryStats.topicMasteryStats[i].topicId.ToString() == topic)
                {
                    for (int j = 0; j < topicsMasteryStats.topicMasteryStats[i].studentsStats.Length; j++)
                    {

                        studentStats.Add(new StudentStats(topicsMasteryStats.topicMasteryStats[i].studentsStats[j].name, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].classGroup
                            , topicsMasteryStats.topicMasteryStats[i].studentsStats[j].year, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].semester, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalAttempts
                            , topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalCorrect, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].correctness));
                    }
                }

            }
            List<StudentStats> temp = new List<StudentStats>() { };

            if (classgrp != "-1")
            {
                for (int i = 0; i < studentStats.Count; i++)
                {
                    if (studentStats[i].classGroup.ToString() == classgrp)
                    {
                        temp.Add(studentStats[i]);
                    }
                }
                studentStats = temp;
            }



            temp = new List<StudentStats>() { };
            for (int i = 0; i < studentStats.Count; i++)
            {
                if (studentStats[i].year.ToString() == year)
                {
                    temp.Add(studentStats[i]);
                }
            }
            studentStats = temp;

            List<string> sems = new List<string>() { };
            for (int i = 0; i < studentStats.Count; i++)
            {
                if (studentStats[i].semester == 1 && (sems.Count == 0 || sems[0] == "Semester 2"))
                {
                    sems.Add("Semester 1");
                }
                if (studentStats[i].semester == 2 && (sems.Count == 0 || sems[0] == "Semester 1"))
                {
                    sems.Add("Semester 2");
                }
                if (sems.Count == 2)
                    break;
            }

            dropdown.options.Clear();
            dropdown.AddOptions(sems);
            itemClicked = 0;
        }



    }
}
