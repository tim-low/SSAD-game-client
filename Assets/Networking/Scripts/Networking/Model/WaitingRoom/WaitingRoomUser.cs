using Game_Server.Util;

namespace SuperSad.Model
{
    public class WaitingRoomUser
    {
        public string Token { get; private set; }
        public string Username { get; private set; }
        public int Head { get; private set; }

        public WaitingRoomUser(SerializeReader reader)
        {
            Token = reader.ReadUnicodeStatic(44);
            Username = reader.ReadUnicodeStatic(13);
            Head = reader.ReadByte();
        }
    }
}