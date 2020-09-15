using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperSad.Model;
using System.Linq;
using System.Text.RegularExpressions;

public class GraphDisplay : MonoBehaviour {

    [Header("Bar")]
    [SerializeField] Bar barPrefab;
    //[SerializeField] List<Color> colors;


    [Header("Dropdown")]
    [SerializeField] Dropdownmanager topicDropdownManager;
    [SerializeField] Dropdownmanager statisticsDropdownManager;
    [SerializeField] Dropdownmanager classDropdownManager;
    [SerializeField] Dropdownmanager quizDropdownManager;
    [SerializeField] Dropdownmanager questionDropdownManager;
    [SerializeField] Dropdownmanager yearDropdownManager;
    [SerializeField] Dropdownmanager semDropdownManager;
    [SerializeField] Dropdownmanager classPerfYearDropdownManager;


    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] Text responseText;
    [SerializeField] Text customQuizQuestionText;
    private List<Transform> transforms;
    private List<Bar> bars = new List<Bar>() { };
    private float chartHeight;
    private float maxValue;
    private List<float> normalizedValue =  new List<float>() { };
    [SerializeField] private List<string> hoverString;
    string csn;

    int statsOpt = 0;
    int topicOpt = 0;
    int classOpt = 0;
    int quizOpt = 0;
    int questionOpt = 0;
    int yearOpt = 0;
    int classyearOpt = 0;
    int semOpt = 0;
    string stats;
    string topic;
    string classgrp;
    string quiz;
    string question;
    string year;
    string sem;
    string classyear;

    bool cohortFlag = false;

    private List<float> ydata = new List<float>() { };
    private List<string> xdata = new List<string>() { };
    [SerializeField] private Color barColor;
    [SerializeField] private string dataToGet;
    List<List<StudentStats>> byClass = new List<List<StudentStats>>() { };
    List<StudentStats> subByClass = new List<StudentStats>() { };
    List<int> marks;
    List<StudentStats> studentStats;
    CustomQuizzesStats cusQuizzesStats;
    CustomQuizStatsGET customQuizStatsGET;
    TopicsMasteryStats topicsMasteryStats;


    // Use this for initialization
    /*    void Start()
        {
            customQuizStatsGET.
        }*/


    public void GetData(CustomQuizzesStats cusQuizzesStats)
    {
        this.cusQuizzesStats = cusQuizzesStats;
        GenerateGraph();
    }

    public void GetStudData(TopicsMasteryStats topicsMasteryStats)
    {
        this.topicsMasteryStats = topicsMasteryStats;
        GenerateGraph();
    }


    public void GenerateGraph()
    {

        ydata = new List<float>() { };
        xdata = new List<string>() { };
        hoverString = new List<string>() { };
        if (statisticsDropdownManager != null)
            stats = statisticsDropdownManager.GetComponent<Dropdown>().options[statsOpt].text;
        else
            stats = null;

        if (topicDropdownManager != null)
        {
            topic = topicDropdownManager.GetComponent<Dropdown>().options[topicOpt].text;
            if (topic == "Requirements")
                topic = "1";
            else if (topic == "Design")
                topic = "2";
            else if (topic == "Implementation")
                topic = "3";
            else if (topic == "Testing")
                topic = "4";
            else if (topic == "Maintenance")
                topic = "5";
            else if (topic == "Miscellaneous")
                topic = "6";
            else if (topic == "Others")
                topic = "7";
            //topic = topic.Substring(topic.Length - 1);
        }

        else
            topic = null;

        if (classDropdownManager != null)
            classgrp = classDropdownManager.GetComponent<Dropdown>().options[classOpt].text;
        else
            classgrp = null;

        if (quizDropdownManager != null)
            quiz = quizDropdownManager.GetComponent<Dropdown>().options[quizOpt].text;
        else
            quiz = null;

        if (questionDropdownManager != null)
        {
            question = questionDropdownManager.GetComponent<Dropdown>().options[questionOpt].text;
            //question = question.Substring(question.Length - 1);
            question = Regex.Replace(question, "[^0-9.]", "");
        }


        else
            question = null;

        if (yearDropdownManager != null)
            year = yearDropdownManager.GetComponent<Dropdown>().options[yearOpt].text;
        else
            year = null;
        if (classPerfYearDropdownManager != null)
        {
            classyear = classPerfYearDropdownManager.GetComponent<Dropdown>().options[yearOpt].text;
            year = classyear;
        }


        if (semDropdownManager != null)
        {
            sem = semDropdownManager.GetComponent<Dropdown>().options[semOpt].text;
            sem = sem.Substring(sem.Length - 1);
        }

        else
            sem = null;

        if (dataToGet == "CustomQuiz")
        {

            marks = new List<int>() { };
            int count;
            for (int i = 0; i < cusQuizzesStats.quizStat.Length; i++)
            {
                if (cusQuizzesStats.quizStat[i].quizName == quiz)
                {
                    for (int j = 0; j < cusQuizzesStats.quizStat[i].studentsAttempted.Length; j++)
                    {
                        marks.Add(cusQuizzesStats.quizStat[i].studentsAttempted[j].numCorrect);
                    }
                    int highestMark = marks.Max();
                    for(int j = 0; j < highestMark+1; j++)
                    {
                        xdata.Add(j.ToString());
                        count = 0;
                        for(int k = 0; k < marks.Count; k++)
                        {
                            if(marks[k] == j)
                            {
                                count++;
                            }
                        }
                        ydata.Add(count);
                    }

                }

            }
        }

        if (dataToGet == "CustomQuizQn")
        {
            xdata = new List<string>(){ "Correct Ans", "Wrong Ans 1", "Wrong Ans 2", "Wrong Ans 3"};

            for (int i = 0; i < cusQuizzesStats.quizStat.Length; i++)
            {
                Debug.Log(cusQuizzesStats.quizStat[i].quizName);

                if (cusQuizzesStats.quizStat[i].quizName == quiz)
                {
                    for (int j = 0; j < cusQuizzesStats.quizStat[i].questionStats.Length; j++)
                    {
                        if(cusQuizzesStats.quizStat[i].questionStats[j].questionId.ToString() == question)
                        {
                            ydata.Add(cusQuizzesStats.quizStat[i].questionStats[j].correctAnsStudentsCount);
                            ydata.Add(cusQuizzesStats.quizStat[i].questionStats[j].wrongAns1StudentsCount);
                            ydata.Add(cusQuizzesStats.quizStat[i].questionStats[j].wrongAns2StudentsCount);
                            ydata.Add(cusQuizzesStats.quizStat[i].questionStats[j].wrongAns3StudentsCount);
                            hoverString.Add(cusQuizzesStats.quizStat[i].questionStats[j].correctAnsText);
                            hoverString.Add(cusQuizzesStats.quizStat[i].questionStats[j].wrongAns1Text);
                            hoverString.Add(cusQuizzesStats.quizStat[i].questionStats[j].wrongAns2Text);
                            hoverString.Add(cusQuizzesStats.quizStat[i].questionStats[j].wrongAns3Text);
                            customQuizQuestionText.text = cusQuizzesStats.quizStat[i].questionStats[j].questionText;
                            Debug.Log(cusQuizzesStats.quizStat[i].questionStats[j].correctAnsText);
                            break;
                        }



                    }
                    break;
                }
            }
            Debug.Log(ydata.Count);
        }


        if(dataToGet == "CohortPerf")
        {
            cohortFlag = true;
            for (int i = 0; i < topicsMasteryStats.topicMasteryStats.Length; i++)
            {
                xdata = new List<string>(){"Requirements", "Design", "Implementation", "Testing", "Maintenance", "Miscellaneous"};
                switch (stats)
                {
                    case "Highest":
                        ydata.Add(topicsMasteryStats.topicMasteryStats[i].highestCorrectness);
                        break;
                    case "Lowest":
                        ydata.Add(topicsMasteryStats.topicMasteryStats[i].lowestCorrectness);
                        break;
                    case "Mean":
                        ydata.Add(topicsMasteryStats.topicMasteryStats[i].meanCorrectness);
                        break;
                    case "Median":
                        ydata.Add(topicsMasteryStats.topicMasteryStats[i].medianCorrectness);
                        break;
                    case "Standard Deviation":
                        ydata.Add(topicsMasteryStats.topicMasteryStats[i].sd);
                        break;
                }


            }
        }


        if(dataToGet == "ClassPerf")
        {
            cohortFlag = true;
            studentStats = new List<StudentStats>() { };
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

            for (int i = 0; i < studentStats.Count; i++)
            {
                if (studentStats[i].year.ToString() == year && studentStats[i].semester.ToString() == sem)
                {
                    temp.Add(studentStats[i]);
                }
            }
            studentStats = temp;
            if (studentStats.Count == 0)
            {
                responseText.text = "No Statistics";
                responseText.gameObject.SetActive(true);
            }
            else
                responseText.gameObject.SetActive(false);
            Debug.Log(studentStats.Count);
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

            for(int i = 0; i < byClass.Count; i++)
            {
                xdata.Add(byClass[i][0].classGroup);
            }

            List<float> cors = new List<float>() { };
            for (int i = 0; i < byClass.Count; i++)
            {

                float cor = 0;
                for (int j = 0; j < byClass[i].Count; j++)
                {
                    cor += byClass[i][j].correctness;
                }
                cors.Add(cor / byClass[i].Count);

            }

            List<float> highest = new List<float>() { };

            for (int i = 0; i < byClass.Count; i++)
            {
                highest.Add(byClass[i][0].correctness);
                for (int j = 0; j < byClass[i].Count; j++)
                {
                    if (highest[i] < byClass[i][j].correctness)
                    {
                        highest[i] = byClass[i][j].correctness;
                    }
                }

            }


            List<float> lowest = new List<float>() { };

            for (int i = 0; i < byClass.Count; i++)
            {
                lowest.Add(byClass[i][0].correctness);
                for (int j = 0; j < byClass[i].Count; j++)
                {
                    if (lowest[i] > byClass[i][j].correctness)
                    {
                        lowest[i] = byClass[i][j].correctness;
                    }
                }

            }

            for(int i = 0; i < byClass.Count;i++)
            {
                switch (stats)
                {
                    case "Highest":
                        ydata.Add(highest[i]);
                        break;
                    case "Lowest":
                        ydata.Add(lowest[i]);
                        break;
                    case "Correctness":
                        ydata.Add(cors[i]);
                        break;

                }
            }
        }

        if(dataToGet == "StudentPerf")
        {
            cohortFlag = true;
            studentStats = new List<StudentStats>() { };
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

            for (int i = 0; i < studentStats.Count; i++)
            {
                if (studentStats[i].year.ToString() == year && studentStats[i].semester.ToString() == sem)
                {
                    temp.Add(studentStats[i]);
                }
            }
            studentStats = temp;
            if (studentStats.Count == 0)
            {
                responseText.text = "No Statistics";
                responseText.gameObject.SetActive(true);
            }
            else
                responseText.gameObject.SetActive(false);
            Debug.Log(studentStats.Count);
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
            for (int i = 0; i < byClass.Count; i++)
            {
                if (byClass[i][0].classGroup == classgrp)
                {
                   
                    for (int j = 0; j < byClass[i].Count; j++)
                    {
                        xdata.Add(byClass[i][j].name);
                        if(byClass[i][j].totalAttempts == 0)
                        {
                            ydata.Add(-1);
                        }
                        else
                        ydata.Add(byClass[i][j].correctness);

                    }


                }
            }

        }

        if(responseText != null)
        {
            if (xdata.Count == 0)
            {
                responseText.text = "No Statistics";
                responseText.gameObject.SetActive(true);
            }
            else
            {
                responseText.gameObject.SetActive(false);
            }
        }


        chartHeight = GetComponent<RectTransform>().sizeDelta.y;
        if(ydata.Count != 0)
        {
            maxValue = ydata.Max();
        }
        else
        {
            maxValue = 0;
        }


        normalizedValue.Clear();
        if (maxValue <= 0)
        {
            for (int i = 0; i < xdata.Count; i++)
            {
                      normalizedValue.Add(0);

            }
        }
        else
        {
            for (int i = 0; i < ydata.Count; i++)
            {
                  normalizedValue.Add(ydata[i] / maxValue);
            }
        }
        DisplayGraph(normalizedValue);


    }


    public void DisplayGraph(List<float> normalized)
    {
        for (int i = 0; i < normalized.Count; i++)
        {
            Bar newBar = Instantiate<Bar>(barPrefab, transform);
            newBar.transform.SetParent(transform);
            newBar.bar.color = barColor;
            RectTransform rt = newBar.bar.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, chartHeight * normalized[i]*0.9f);
            newBar.xText.text = xdata[i];
            if (ydata[i] < 0)
                newBar.count.text = "NA";
            else if(cohortFlag)
                newBar.count.text = ydata[i].ToString() + " %";
            else
                newBar.count.text = ydata[i].ToString();
            bars.Add(newBar);
            // newBar.bar.color = colors[i];
            if (dataToGet == "CustomQuizQn" && normalized.Count == hoverString.Count)
            {
                newBar.GetComponent<OnHoverUI>().hoverText.text = hoverString[i];
                newBar.GetComponent<OnHoverUI>().hoverObj.transform.SetParent(transform.root);
            }
        }
        cohortFlag = false;
    }

    public void reloadTopicData()
    {
        Clear();
        topicOpt = topicDropdownManager.itemClicked;

        if (topicDropdownManager != null)
        {
            topic = topicDropdownManager.GetComponent<Dropdown>().options[topicOpt].text;
            if (topic == "Requirements")
                topic = "1";
            else if (topic == "Design")
                topic = "2";
            else if (topic == "Implementation")
                topic = "3";
            else if (topic == "Testing")
                topic = "4";
            else if (topic == "Maintenance")
                topic = "5";
            else if (topic == "Miscellaneous")
                topic = "6";
            else if (topic == "Others")
                topic = "7";
        }
        else
            topic = null;


        GenerateGraph();
    }


    public void reloadStatsData()
    {
        Clear();
        statsOpt = statisticsDropdownManager.itemClicked;
        GenerateGraph();
    }

    public void reloadClassData()
    {
        Clear();
        classOpt = classDropdownManager.itemClicked;
        if (topicDropdownManager != null)
        {
            topic = topicDropdownManager.GetComponent<Dropdown>().options[topicOpt].text;
            if (topic == "Requirements")
                topic = "1";
            else if (topic == "Design")
                topic = "2";
            else if (topic == "Implementation")
                topic = "3";
            else if (topic == "Testing")
                topic = "4";
            else if (topic == "Maintenance")
                topic = "5";
            else if (topic == "Miscellaneous")
                topic = "6";
            else if (topic == "Others")
                topic = "7";
        }
        else
            topic = null;

        if (classDropdownManager != null)
            classgrp = classDropdownManager.GetComponent<Dropdown>().options[classOpt].text;
        else
            classgrp = null;


        if (yearDropdownManager != null)
        {
            yearDropdownManager.FilterByYear(topic, classgrp);
            yearOpt = yearDropdownManager.itemClicked;
            year = yearDropdownManager.GetComponent<Dropdown>().options[yearOpt].text;
        }

        if (semDropdownManager != null)
        {
            semDropdownManager.FilterSemByYear(topic, classgrp, year);
            semOpt = semDropdownManager.itemClicked;
        }
        GenerateGraph();
    }


    public void reloadClassYearData()
    {
        Clear();
        yearOpt = classPerfYearDropdownManager.itemClicked;

        if (topicDropdownManager != null)
        {
            topic = topicDropdownManager.GetComponent<Dropdown>().options[topicOpt].text;
            if (topic == "Requirements")
                topic = "1";
            else if (topic == "Design")
                topic = "2";
            else if (topic == "Implementation")
                topic = "3";
            else if (topic == "Testing")
                topic = "4";
            else if (topic == "Maintenance")
                topic = "5";
            else if (topic == "Miscellaneous")
                topic = "6";
            else if (topic == "Others")
                topic = "7";
        }
        else
            topic = null;
        if (classPerfYearDropdownManager != null)
            year = classPerfYearDropdownManager.GetComponent<Dropdown>().options[yearOpt].text;

        if (semDropdownManager != null)
        {
            semDropdownManager.FilterSemByYear(topic, "-1", year);
            semOpt = semDropdownManager.itemClicked;
        }

        GenerateGraph();
    }
    public void reloadYearData()
    {
        Clear();
        yearOpt = yearDropdownManager.itemClicked;
        if (topicDropdownManager != null)
        {
            topic = topicDropdownManager.GetComponent<Dropdown>().options[topicOpt].text;
            if (topic == "Requirements")
                topic = "1";
            else if (topic == "Design")
                topic = "2";
            else if (topic == "Implementation")
                topic = "3";
            else if (topic == "Testing")
                topic = "4";
            else if (topic == "Maintenance")
                topic = "5";
            else if (topic == "Miscellaneous")
                topic = "6";
            else if (topic == "Others")
                topic = "7";
        }
        else
            topic = null;

        if (classDropdownManager != null)
            classgrp = classDropdownManager.GetComponent<Dropdown>().options[classOpt].text;
        else
            classgrp = null;

        if (yearDropdownManager != null)
            year = yearDropdownManager.GetComponent<Dropdown>().options[yearOpt].text;
        else
            year = null;

        if (semDropdownManager != null)
        {
            semDropdownManager.FilterSemByYear(topic, classgrp, year);
            semOpt = semDropdownManager.itemClicked;
        }


        GenerateGraph();
    }

    public void reloadSemData()
    {
        Clear();
        semOpt = semDropdownManager.itemClicked;
        GenerateGraph();
    }

    public void reloadQuizData()
    {
        Clear();
        quizOpt = quizDropdownManager.itemClicked;
        quiz = quizDropdownManager.GetComponent<Dropdown>().options[quizOpt].text;
        if (questionDropdownManager != null)
            questionDropdownManager.populateQuestionDropdown(quiz);
        GenerateGraph();
    }

    public void reloadQuestionData()
    {
        Clear();
        questionOpt = questionDropdownManager.itemClicked;
        GenerateGraph();
    }
    public void Clear()
    {
        if (bars != null)
        {
            foreach (Bar entry in bars)
            {

                Destroy(entry.gameObject);
            }
            bars.Clear();

        }

    }

}
