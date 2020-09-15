using System;
//using System.Numerics;
//using SampleClient.Object;
using Game_Server.Util;

/*
 * Room Model for Lobby display
 */
namespace SuperSad.Model
{
    public class Room
    {
        // public Room Room {get; private set;}

        public string RoomId {get; set;} // 36
        public string RoomName {get; set;} // 40
        public bool IsLocked {get; set;} // 1
        //public int NoOfQuestion {get; set;}
        public int NoOfUser {get; set;}
        public int MaxUser {get; set;}
        public bool IsInGame { get; set; }

        public bool IsFull()
        {
            return (NoOfUser == MaxUser);
        }

        public Room(SerializeReader reader)
        {
            RoomId = reader.ReadUnicodeStatic(36);
            RoomName = reader.ReadUnicodeStatic(40);
            IsLocked = reader.ReadBoolean();
            //NoOfQuestion = reader.ReadInt32();
            NoOfUser = reader.ReadInt32();
            MaxUser = reader.ReadInt32();
            IsInGame = reader.ReadBoolean();
        }

    }
}
