using System.Collections.Generic;
//using System.Numerics;
//using SampleClient.Object;

using Game_Server.Model;
using SuperSad.Model;

namespace SuperSad.Networking
{
    public class WorldSelectAck : InPacket
    {
        public int RoomCount {get; private set;}
        public List<Room> Rooms {get; private set;}

        public WorldSelectAck(Packet packet) : base(packet)
        {
            Rooms = new List<Room>();
            RoomCount = Reader.ReadInt32();
            for(int i = 0; i < RoomCount; i++)
            {
                Room room = new Room(Reader);
                Rooms.Add(room);
            }
        }
    }
}
