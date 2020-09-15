using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Model
{
    [System.Serializable]
    public class TopicMasteryStats
    {
        public int topicId;
        public float meanCorrectness;
        public float medianCorrectness;
        public float sd;
        public int highestCorrectness; //highestCorrectness that a student obtained
        public int lowestCorrectness;
        public StudentStats[] studentsStats;

        public TopicMasteryStats(int topicId, float meanCorrectness, float medianCorrectness, float sd, int highestCorrectness, int lowestCorrectness, StudentStats[] studsStats)
        {
            this.topicId = topicId;
            this.meanCorrectness = meanCorrectness;
            this.medianCorrectness = medianCorrectness;
            this.sd = sd;
            this.highestCorrectness = highestCorrectness;
            this.lowestCorrectness = lowestCorrectness;
            this.studentsStats = studsStats;
        }
    }

    [System.Serializable]
    public class TopicsMasteryStats
    {
        public TopicMasteryStats[] topicMasteryStats;
    }
}