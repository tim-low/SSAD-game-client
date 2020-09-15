using Game_Server.Util;

using SuperSad.Gameplay;

namespace SuperSad.Model
{
    public class QuizReward
    {
        public string Token { get; private set; }
        public bool IsCorrect { get; private set; }
        public int StepsAwarded { get; private set; }
        public ItemType ItemAwarded { get; private set; }
        public bool IsTimeout { get; private set; }

        public QuizReward(SerializeReader reader)
        {
            Token = reader.ReadUnicodeStatic(44);
            IsCorrect = reader.ReadBoolean();
            StepsAwarded = reader.ReadInt32();
            ItemAwarded = (ItemType)reader.ReadInt32();
            IsTimeout = reader.ReadBoolean();
        }
    }

}