using System.Collections;
using System.Collections.Generic;
using Game_Server.Util;

namespace SuperSad.Model
{
    public class GameScore {

        public string Token { get; private set; }
        public int Score { get; private set; }
        public int AnswerCorrectly { get; private set; }
        public int ExperienceGained { get; private set; }
        public int CurrentExperience { get; private set; }
        public int CurrentLevel { get; private set; }
        public int NumLootBox { get; private set; }

        public GameScore(SerializeReader reader)
        {
            Token = reader.ReadUnicodeStatic(44);
            Score = reader.ReadInt32();
            AnswerCorrectly = reader.ReadInt32();
            ExperienceGained = reader.ReadInt32();
            CurrentExperience = reader.ReadInt32();
            CurrentLevel = reader.ReadInt32();
            NumLootBox = reader.ReadByte();
        }
    }

}