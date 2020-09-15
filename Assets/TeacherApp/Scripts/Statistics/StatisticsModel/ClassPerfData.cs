using SuperSad.Model;
using System.Collections.Generic;

public class ClassPerfData
{
    public string classGroup;
    public int year;
    public int semester;

    public int totalAttempts;
    public int totalCorrect;

    public ClassPerfData() { }

    public ClassPerfData(string classGroup, int year, int semester)
    {
        this.classGroup = classGroup;
        this.year = year;
        this.semester = semester;
        this.totalAttempts = 0;
        this.totalCorrect = 0;
    }

    public ClassPerfData(string classGroup, int year, int semester, int totalAttempts, int totalCorrect)
    {
        this.classGroup = classGroup;
        this.year = year;
        this.semester = semester;
        this.totalAttempts = totalAttempts;
        this.totalCorrect = totalCorrect;
    }

    public bool CheckClassGrpYearSem(string classGroup, int year, int semester)
    {
        return (this.classGroup == classGroup && this.year == year && this.semester == semester);
    }

    public void AddStudentStats(StudentStats studentStats)
    {
        totalAttempts += studentStats.totalAttempts;
        totalCorrect += studentStats.totalCorrect;
    }

    public float ClassCorrectness()
    {
        return (totalAttempts == 0 ) ? 0 : (((float)totalCorrect / (float)totalAttempts) * 100);
    }
}