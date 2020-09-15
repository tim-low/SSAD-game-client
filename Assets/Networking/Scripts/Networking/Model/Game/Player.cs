using System.Collections.Generic;
using Game_Server.Util;

using SuperSad.Gameplay;

namespace SuperSad.Model
{

    public class Player {

        public string Token {get; private set; }
        public string Username { get; private set; }
        public byte PositionX {get; private set; }
        public byte PositionY {get; private set; }
        public byte Color {get; private set; }
        public int StepsLeft {get; private set; }
        public int ItemCount {get; private set; }

        public List<ItemType> Items {get; private set;}

        public Player(SerializeReader reader)
        {
            Items = new List<ItemType>();

            Token = reader.ReadUnicodeStatic(44);
            Username = reader.ReadUnicodeStatic(13);
            PositionX = reader.ReadByte();
            PositionY = reader.ReadByte();
            Color = reader.ReadByte();
            StepsLeft = reader.ReadInt32();
            ItemCount = reader.ReadInt32();
            for(int i = 0; i < ItemCount; i++)
            {
                ItemType item = (ItemType)reader.ReadInt32();
                Items.Add(item);
            }
        }

        public void SetSteps(int steps)
        {
            StepsLeft = steps;
        }

        public int DecrementSteps()
        {
            StepsLeft -= 1;
            return StepsLeft;
        }
    }

}