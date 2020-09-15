using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperSad.Model;
using UnityEngine.UI;

public class TableDisplay : MonoBehaviour
{
    [Header("Dropdown")]
    [SerializeField] Dropdownmanager topicDropdownManager;
    [SerializeField] Dropdownmanager classDropdownManager;
    [SerializeField] Dropdownmanager quizDropdownManager;
    [SerializeField] Dropdownmanager yearDropdownManager;
    [SerializeField] Dropdownmanager semDropdownManager;
    [SerializeField] Dropdownmanager classPerfYearDropdownManager;
    [Header("UI Elements")]
    [SerializeField] GameObject row;
    [SerializeField] GameObject verticalLayoutGroup;
    [SerializeField] Text responseText;

    [SerializeField] string dataToGet;
    GameObject newRow;
    List<GameObject> goList = new List<GameObject>() { };
    List<StudentStats> studentStats;
    List<float> cors= new List<float>() { };
    List<List<StudentStats>> byClass = new List<List<StudentStats>>() { };
    List<StudentStats> subByClass = new List<StudentStats>() { };

    string stats;
    string topic;
    string classgrp;
    string quiz;
    string year;
    string sem;
    string classyear;
    int topicOpt = 0;
    int classOpt = 0;
    int quizOpt = 0;
    int yearOpt = 0;
    int semOpt = 0;
    //test
    List<string> test1 = new List<string>() { "1", "2", "3" };
    CustomQuizzesStats cusQuizzesStats;
    TopicsMasteryStats topicsMasteryStats;

    /*    void Start()
        {
            loadTable(null);
        }*/

    public void GetData(CustomQuizzesStats cusQuizzesStats)
    {
        this.cusQuizzesStats = cusQuizzesStats;
        loadTable();
    }

    public void GetStudData(TopicsMasteryStats topicsMasteryStats)
    {
        this.topicsMasteryStats = topicsMasteryStats;
        loadTable();
    }


    public void loadTable()
    {

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
            /*            Debug.Log(topic);
                        Debug.Log(topic.Substring(topic.Length - 1));
                        topic = topic.Substring(topic.Length - 1);*/
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
            Debug.Log("CustomQuiz");
            //Debug.Log(cusQuizzesStats.quizStat.Length);
            for (int i = 0; i < cusQuizzesStats.quizStat.Length; i++)
            {
                newRow = Instantiate(row);
                newRow.transform.SetParent(verticalLayoutGroup.transform, false);
                newRow.gameObject.SetActive(true);
                newRow.transform.Find("QuizText").GetComponent<Text>().text = cusQuizzesStats.quizStat[i].quizName;
                newRow.transform.Find("HighestText").GetComponent<Text>().text = cusQuizzesStats.quizStat[i].highestScore.ToString();
                newRow.transform.Find("LowestText").GetComponent<Text>().text = cusQuizzesStats.quizStat[i].lowestScore.ToString();
                newRow.transform.Find("MeanText").GetComponent<Text>().text = cusQuizzesStats.quizStat[i].mean.ToString();
                newRow.transform.Find("SDText").GetComponent<Text>().text = cusQuizzesStats.quizStat[i].standardDeviation.ToString();
                goList.Add(newRow);
            }

        }

        if (dataToGet == "CustomQuizQn")
        {

            Debug.Log("CustomQuizQn");
            // Debug.Log(cusQuizzesStats.quizStat[0].questionStats.Length);
            for (int j = 0; j < cusQuizzesStats.quizStat.Length; j++)
            { 
                if(cusQuizzesStats.quizStat[j].quizName == quiz)
                {
                    for (int i = 0; i < cusQuizzesStats.quizStat[j].questionStats.Length; i++)
                    {
                        newRow = Instantiate(row);
                        newRow.transform.SetParent(verticalLayoutGroup.transform, false);
                        newRow.gameObject.SetActive(true);
                        Transform qnIdText = newRow.transform.Find("QnIdText");
                        Transform correctAnsStudentsCount = newRow.transform.Find("CorrectAnsStudentsCount");
                        Transform wrongAns1StudentsCount = newRow.transform.Find("WrongAns1StudentsCount");
                        Transform wrongAns2StudentsCount = newRow.transform.Find("WrongAns2StudentsCount");
                        Transform wrongAns3StudentsCount = newRow.transform.Find("WrongAns3StudentsCount");
                        qnIdText.GetComponent<Text>().text = cusQuizzesStats.quizStat[j].questionStats[i].questionId.ToString();
                        newRow.transform.Find("CorText").GetComponent<Text>().text = cusQuizzesStats.quizStat[j].questionStats[i].correctness.ToString();
                        correctAnsStudentsCount.GetComponent<Text>().text = cusQuizzesStats.quizStat[j].questionStats[i].correctAnsStudentsCount.ToString();
                        wrongAns1StudentsCount.GetComponent<Text>().text = cusQuizzesStats.quizStat[j].questionStats[i].wrongAns1StudentsCount.ToString();
                        wrongAns2StudentsCount.GetComponent<Text>().text = cusQuizzesStats.quizStat[j].questionStats[i].wrongAns2StudentsCount.ToString();
                        wrongAns3StudentsCount.GetComponent<Text>().text = cusQuizzesStats.quizStat[j].questionStats[i].wrongAns3StudentsCount.ToString();

                        qnIdText.GetComponent<OnHoverUI>().hoverText.text = cusQuizzesStats.quizStat[j].questionStats[i].questionText;
                        qnIdText.GetComponent<OnHoverUI>().hoverObj.transform.SetParent(transform.root);

                        correctAnsStudentsCount.GetComponent<OnHoverUI>().hoverText.text = cusQuizzesStats.quizStat[j].questionStats[i].correctAnsText;
                        correctAnsStudentsCount.GetComponent<OnHoverUI>().hoverObj.transform.SetParent(transform.root);

                        wrongAns1StudentsCount.GetComponent<OnHoverUI>().hoverText.text = cusQuizzesStats.quizStat[j].questionStats[i].wrongAns1Text;
                        wrongAns1StudentsCount.GetComponent<OnHoverUI>().hoverObj.transform.SetParent(transform.root);

                        wrongAns2StudentsCount.GetComponent<OnHoverUI>().hoverText.text = cusQuizzesStats.quizStat[j].questionStats[i].wrongAns2Text;
                        wrongAns2StudentsCount.GetComponent<OnHoverUI>().hoverObj.transform.SetParent(transform.root);

                        wrongAns3StudentsCount.GetComponent<OnHoverUI>().hoverText.text = cusQuizzesStats.quizStat[j].questionStats[i].wrongAns3Text;
                        wrongAns3StudentsCount.GetComponent<OnHoverUI>().hoverObj.transform.SetParent(transform.root);

                        goList.Add(newRow);
                    }    
                }
        
            }

        }

        if(dataToGet == "CohortPerf")
        {
            List<string> topics = new List<string>() { "Requirements", "Design", "Implementation", "Testing", "Maintenance", "Miscellaneous" };
            for(int i = 0; i < topicsMasteryStats.topicMasteryStats.Length; i++)
            {
                newRow = Instantiate(row);
                newRow.transform.SetParent(verticalLayoutGroup.transform, false);
                newRow.gameObject.SetActive(true);
                newRow.transform.Find("TopicText").GetComponent<Text>().text = topics[i];
                newRow.transform.Find("HighestText").GetComponent<Text>().text = topicsMasteryStats.topicMasteryStats[i].highestCorrectness.ToString();
                newRow.transform.Find("LowestText").GetComponent<Text>().text = topicsMasteryStats.topicMasteryStats[i].lowestCorrectness.ToString();
                newRow.transform.Find("MeanText").GetComponent<Text>().text = topicsMasteryStats.topicMasteryStats[i].meanCorrectness.ToString();
                newRow.transform.Find("SDText").GetComponent<Text>().text = topicsMasteryStats.topicMasteryStats[i].sd.ToString();
                newRow.transform.Find("MedianText").GetComponent<Text>().text = topicsMasteryStats.topicMasteryStats[i].medianCorrectness.ToString();

                goList.Add(newRow);
            }
        }

        if (dataToGet == "ClassPerf")
        {
            studentStats = new List<StudentStats>() { };
            for (int i = 0; i < topicsMasteryStats.topicMasteryStats.Length; i++)
            {
                if (topicsMasteryStats.topicMasteryStats[i].topicId.ToString() == topic)
                {
                    for (int j = 0; j < topicsMasteryStats.topicMasteryStats[i].studentsStats.Length; j++)
                    {
                        Debug.Log(topicsMasteryStats.topicMasteryStats[i].studentsStats.Length);
                        studentStats.Add(new StudentStats(topicsMasteryStats.topicMasteryStats[i].studentsStats[j].name, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].classGroup
                            , topicsMasteryStats.topicMasteryStats[i].studentsStats[j].year, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].semester, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalAttempts
                            , topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalCorrect, topicsMasteryStats.topicMasteryStats[i].studentsStats[j].correctness));
                    }
                }

            }
            List<StudentStats> temp = new List<StudentStats>() { };

            for(int i = 0; i < studentStats.Count; i++)
            {
                if(studentStats[i].year.ToString() == year && studentStats[i].semester.ToString() == sem)
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
            Debug.Log(byClass.Count);
            cors = new List<float>() { };
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

            for (int i = 0; i < byClass.Count; i++)
            {

                newRow = Instantiate(row);
                newRow.transform.SetParent(verticalLayoutGroup.transform, false);
                newRow.gameObject.SetActive(true);
                newRow.transform.Find("ClassText").GetComponent<Text>().text = byClass[i][0].classGroup;
                newRow.transform.Find("HighestText").GetComponent<Text>().text = highest[i].ToString();
                newRow.transform.Find("LowestText").GetComponent<Text>().text = lowest[i].ToString();
                newRow.transform.Find("CorText").GetComponent<Text>().text = cors[i].ToString();
                goList.Add(newRow);
            }
            if (goList.Count == 0)
            {
                responseText.text = "No Statistics";
                responseText.gameObject.SetActive(true);
            }
            else
            {
                responseText.gameObject.SetActive(false);
            }

        }

            if(dataToGet == "StudentPerf")
            {

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
            Debug.Log(studentStats.Count);
            Debug.Log(year);
            if (studentStats.Count == 0)
            {
                responseText.text = "No Statistics";
                responseText.gameObject.SetActive(true);
            }
            else
                responseText.gameObject.SetActive(false);
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

                for(int i = 0; i < byClass.Count; i ++)
                {
                    if(byClass[i][0].classGroup == classgrp)
                    {
                    Debug.Log(byClass[i][0].classGroup);
                    for (int j = 0; j < byClass[i].Count; j++)
                    {

                        newRow = Instantiate(row);
                            newRow.transform.SetParent(verticalLayoutGroup.transform, false);
                            newRow.gameObject.SetActive(true);
                            newRow.transform.Find("StudentText").GetComponent<Text>().text = byClass[i][j].name;
                            newRow.transform.Find("CorText").GetComponent<Text>().text = byClass[i][j].correctness.ToString();
                            newRow.transform.Find("TotalAttempt").GetComponent<Text>().text = byClass[i][j].totalAttempts.ToString();
                            newRow.transform.Find("TotalCorrect").GetComponent<Text>().text = byClass[i][j].totalCorrect.ToString();
                            goList.Add(newRow);
                    }
                        break;

                    }
                }
                if(goList.Count == 0)
                {
                    responseText.text = "No Statistics";
                    responseText.gameObject.SetActive(true);
                 }
            else
            {
                responseText.gameObject.SetActive(false);
            }

            }


        

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

        loadTable();
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

        loadTable();
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
        else
            year = null;
        if (semDropdownManager != null)
        {
            semDropdownManager.FilterSemByYear(topic, "-1", year);
            semOpt = semDropdownManager.itemClicked;
        }
        loadTable();
    }
    public void reloadQuizData()
    {
        Clear();

        quizOpt = quizDropdownManager.itemClicked;
        loadTable();
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

        loadTable();
    }

    public void reloadSemData()
    {
        Clear();

        semOpt = semDropdownManager.itemClicked;
        loadTable();
    }
    public void Clear()
    {
        if (goList != null)
        {
            foreach (GameObject entry in goList)
            {

                Destroy(entry.gameObject);
            }
            goList.Clear();

        }

    }
}




