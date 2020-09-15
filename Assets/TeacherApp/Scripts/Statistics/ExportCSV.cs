using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using SuperSad.Model;
using UnityEngine.UI;

public class ExportCSV : MonoBehaviour
{
    private List<string[]> rowData = new List<string[]>();
    [SerializeField] TopicsStatsGET topicsStatsGET;
    [SerializeField] CustomQuizStatsGET customQuizStatsGET;
    [SerializeField] GameObject error;
    [SerializeField] Text responseText;
    string csvFileName;

    public void TopicStats()
    {
        rowData.Clear();

        // Creating First row of titles manually..
        csvFileName = "Topics_Statistics";
        string[] rowDataTemp = new string[9];

        rowDataTemp[0] = "topicId";
        rowDataTemp[1] = "MeanCorrectnes";
        rowDataTemp[2] = "MedianCorrectness";
        rowDataTemp[3] = "StandardDeviation";
        rowDataTemp[4] = "HighestCorrectnes";
        rowDataTemp[5] = "LowestCorrectness";
        rowDataTemp[6] = "StudentsAttempted";
        rowDataTemp[7] = "StudentsTotalAttempts";
        rowDataTemp[8] = "StudentsTotalCorrect";

        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < topicsStatsGET.topicsMasteryStats.topicMasteryStats.Length; i++)
        {
            Debug.Log("Testing...");
            rowDataTemp = new string[9];
            Debug.Log(topicsStatsGET.topicsMasteryStats.topicMasteryStats[i]);
            rowDataTemp[0] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].topicId.ToString();
            rowDataTemp[1] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].meanCorrectness.ToString();
            rowDataTemp[2] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].medianCorrectness.ToString();
            rowDataTemp[3] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].sd.ToString();
            rowDataTemp[4] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].highestCorrectness.ToString();
            rowDataTemp[5] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].lowestCorrectness.ToString();
            rowDataTemp[6] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats.Length.ToString();

            int tempTotalAttempts = 0;
            int tempTotalCorrect = 0;
            for (int j = 0; j < topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats.Length; j++)
            {
                tempTotalAttempts += topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalAttempts;
                tempTotalCorrect += topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalCorrect;
            }

            rowDataTemp[7] = tempTotalAttempts.ToString();
            rowDataTemp[8] = tempTotalCorrect.ToString();
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath(csvFileName);
        Debug.Log(filePath);

        try
        {
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

            responseText.text = "File saved under:\n" + filePath;
        }
        catch(Exception e)
        {
            responseText.text = "Error:\n" +e.Message;
        }
        error.SetActive(true);
        rowData.Clear();
    }

    public void ClassesStatistics()
    {
        rowData.Clear();
        // Creating First row of titles manually..
        csvFileName = "Classes_Statistics";
        string[] rowDataTemp = new string[7];

        rowDataTemp[0] = "topicId";
        rowDataTemp[1] = "classGroup";
        rowDataTemp[2] = "year";
        rowDataTemp[3] = "semester";
        rowDataTemp[4] = "TotalAttempts";
        rowDataTemp[5] = "TotalCorrect";
        rowDataTemp[6] = "Correctness"; // TotalAttempts/TotalCorrect * 100

        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < topicsStatsGET.topicsMasteryStats.topicMasteryStats.Length; i++)
        {
            int topicId = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].topicId;
            List<ClassPerfData> classPerfDataList = new List<ClassPerfData>();

            for (int j = 0; j < topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats.Length; j++)
            {
                bool added = false;
                for (int l = 0; l < classPerfDataList.Count; l++)
                {
                    if (classPerfDataList[l].CheckClassGrpYearSem(topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].classGroup,
                            topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].year,
                            topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].semester))
                    {
                        classPerfDataList[l].AddStudentStats(topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j]);
                        added = true;
                    }
                }

                if (!added)
                {
                    ClassPerfData classPerfData = new ClassPerfData(topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].classGroup,
                            topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].year,
                            topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].semester);
                    classPerfData.AddStudentStats(topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j]);
                    classPerfDataList.Add(classPerfData);
                }
            }

            for (int m = 0; m < classPerfDataList.Count; m++)
            {
                rowDataTemp = new string[7];
                rowDataTemp[0] = topicId.ToString();
                rowDataTemp[1] = classPerfDataList[m].classGroup;
                rowDataTemp[2] = classPerfDataList[m].year.ToString();
                rowDataTemp[3] = classPerfDataList[m].semester.ToString();
                rowDataTemp[4] = classPerfDataList[m].totalAttempts.ToString();
                rowDataTemp[5] = classPerfDataList[m].totalCorrect.ToString();
                rowDataTemp[6] = classPerfDataList[m].ClassCorrectness().ToString();
                rowData.Add(rowDataTemp);
            }

        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath(csvFileName);
        Debug.Log(filePath);

        try
        {
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

            responseText.text = "File saved under:\n" + filePath;
        }
        catch (Exception e)
        {
            responseText.text = "Error:\n" + e.Message;
        }
        error.SetActive(true);
        rowData.Clear();
    }

    public void StudentsStatistics()
    {
        rowData.Clear();
        // Creating First row of titles manually..
        csvFileName = "Students_Statistics";
        string[] rowDataTemp = new string[8];

        rowDataTemp[0] = "topicId";
        rowDataTemp[1] = "Name";
        rowDataTemp[2] = "ClassGroup";
        rowDataTemp[3] = "Year";
        rowDataTemp[4] = "Semester";

        rowDataTemp[5] = "TotalAttempts";
        rowDataTemp[6] = "TotalCorrect";
        rowDataTemp[7] = "Correctness";

        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < topicsStatsGET.topicsMasteryStats.topicMasteryStats.Length; i++) //for each topic
        {

            for (int j = 0; j < topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats.Length; j++) // get the student of that topic
            {
                rowDataTemp = new string[8];

                rowDataTemp[0] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].topicId.ToString();
                rowDataTemp[1] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].name;
                rowDataTemp[2] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].classGroup;
                rowDataTemp[3] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].year.ToString();
                rowDataTemp[4] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].semester.ToString();

                rowDataTemp[5] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalAttempts.ToString();
                rowDataTemp[6] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].totalCorrect.ToString();
                rowDataTemp[7] = topicsStatsGET.topicsMasteryStats.topicMasteryStats[i].studentsStats[j].correctness.ToString();
                rowData.Add(rowDataTemp);
            }
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        string filePath = getPath(csvFileName);
        Debug.Log(filePath);

        try
        {
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

            responseText.text = "File saved under:\n" + filePath;
        }
        catch (Exception e)
        {
            responseText.text = "Error:\n" + e.Message;
        }

        error.SetActive(true);
        rowData.Clear();
    }


    public void CustomQuizStatCSV()
    {
        rowData.Clear();

        // Creating First row of titles manually..
        csvFileName = "Custom_Quiz_Statistics";
        string[] rowDataTemp = new string[8];

        rowDataTemp[0] = "quizName";
        rowDataTemp[1] = "highestScore";
        rowDataTemp[2] = "lowestScore";
        rowDataTemp[3] = "mean";
        rowDataTemp[4] = "median";
        rowDataTemp[5] = "standardDeviation";
        rowDataTemp[6] = "NoOfQuestions";
        rowDataTemp[7] = "NoOfStudentsAttempted";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < customQuizStatsGET.cusQuizzesStats.quizStat.Length; i++)
        {
            rowDataTemp = new string[8];
            rowDataTemp[0] = customQuizStatsGET.cusQuizzesStats.quizStat[i].quizName;
            rowDataTemp[1] = customQuizStatsGET.cusQuizzesStats.quizStat[i].highestScore.ToString();
            rowDataTemp[2] = customQuizStatsGET.cusQuizzesStats.quizStat[i].lowestScore.ToString();
            rowDataTemp[3] = customQuizStatsGET.cusQuizzesStats.quizStat[i].mean.ToString();
            rowDataTemp[4] = customQuizStatsGET.cusQuizzesStats.quizStat[i].median.ToString();
            rowDataTemp[5] = customQuizStatsGET.cusQuizzesStats.quizStat[i].standardDeviation.ToString();
            rowDataTemp[6] = customQuizStatsGET.cusQuizzesStats.quizStat[i].questionStats.Length.ToString();
            rowDataTemp[7] = customQuizStatsGET.cusQuizzesStats.quizStat[i].studentsAttempted.Length.ToString();
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath(csvFileName);
        Debug.Log(filePath);

        try
        {
            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

            responseText.text = "File saved under:\n" + filePath;
        }
        catch (Exception e)
        {
            responseText.text = "Error:\n" + e.Message;
        }

        error.SetActive(true);
    }

    public void QnAndStudentsStats(Dropdownmanager dropdownmanager)
    {
        try
        {
            string qnFilePath = QnStats(dropdownmanager.itemClicked);
            string studentFilePath = CustomQuizStudentStats(dropdownmanager.itemClicked);

            responseText.text = "2 files have been exported:\n" + qnFilePath + "\n" + studentFilePath;
        }
        catch (Exception e)
        {
            responseText.text = "Error:\n" + e.Message;
        }

        error.SetActive(true);
    }

    public string QnStats(int quiz)
    {
        rowData.Clear();

        // Creating First row of titles manually..
        csvFileName = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].quizName + "_Question_Statistics";

        string[] rowDataTemp = new string[10];

        rowDataTemp[0] = "questionId";
        rowDataTemp[1] = "correctness";
        rowDataTemp[2] = "correctAnsStudentsCount";
        rowDataTemp[3] = "wrongAns1StudentsCount";
        rowDataTemp[4] = "wrongAns2StudentsCount";
        rowDataTemp[5] = "wrongAns3StudentsCount";
        rowDataTemp[6] = "correctAnswerText";
        rowDataTemp[7] = "wrongAnswer1Text";
        rowDataTemp[8] = "wrongAnswer2Text";
        rowDataTemp[9] = "wrongAnswer3Text";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats.Length; i++)
        {
            rowDataTemp = new string[10];
            rowDataTemp[0] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].questionId.ToString();
            rowDataTemp[1] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].correctness.ToString();
            rowDataTemp[2] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].correctAnsStudentsCount.ToString();
            rowDataTemp[3] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].wrongAns1StudentsCount.ToString();
            rowDataTemp[4] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].wrongAns2StudentsCount.ToString();
            rowDataTemp[5] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].wrongAns3StudentsCount.ToString();
            rowDataTemp[6] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].correctAnsText;
            rowDataTemp[7] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].wrongAns1Text;
            rowDataTemp[8] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].wrongAns2Text;
            rowDataTemp[9] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].questionStats[i].wrongAns3Text;
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath(csvFileName);
        Debug.Log(filePath);

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        rowData.Clear();

        return filePath;
    }

    public string CustomQuizStudentStats(int quiz)
    {
        rowData.Clear();

        // Creating First row of titles manually..
        csvFileName = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].quizName + "_Student_Statistics";

        string[] rowDataTemp = new string[6];

        rowDataTemp[0] = "studentName";
        rowDataTemp[1] = "classGroup";
        rowDataTemp[2] = "year";
        rowDataTemp[3] = "numCorrect";
        rowDataTemp[4] = "correctness";
        rowDataTemp[5] = "answersPicked";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for (int i = 0; i < customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted.Length; i++)
        {
            rowDataTemp = new string[6];
            rowDataTemp[0] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted[i].studentName;
            rowDataTemp[1] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted[i].classGroup;
            rowDataTemp[2] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted[i].year.ToString();
            rowDataTemp[3] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted[i].numCorrect.ToString();
            rowDataTemp[4] = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted[i].correctness.ToString();
            string arrayToString = "[";
            for (int j = 0; j < customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted[i].answerPicked.Length; j++)
            {
                Debug.Log(customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted[i].answerPicked[j].ToString());
                int answerPicked = customQuizStatsGET.cusQuizzesStats.quizStat[quiz].studentsAttempted[i].answerPicked[j] % 4;
                if( answerPicked == 0) { answerPicked = 4; }
                arrayToString += answerPicked.ToString();
                arrayToString += ' ';
                Debug.Log(arrayToString);
            }
            arrayToString += "]";
            Debug.Log(arrayToString);
            rowDataTemp[5] = arrayToString;
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = getPath(csvFileName);

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        rowData.Clear();

        return filePath;
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(string csvFileName)
    {
        if (!Directory.Exists(Application.dataPath + "/CSV/"))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(Application.dataPath + "/CSV/");
        }        
        #if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + csvFileName + ".csv";
        #else
        return Application.dataPath +"/CSV/"+ csvFileName + ".csv";
        #endif
    }
}
