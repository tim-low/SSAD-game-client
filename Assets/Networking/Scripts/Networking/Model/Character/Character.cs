using UnityEngine;
using Game_Server.Util;

namespace SuperSad.Model
{
    public class Character {

        public string Token { get; private set; }
        public string Username { get; private set; }
        public Vector3 Pos { get; private set; }
        public Vector3 Dir { get; private set; }
        public Attributes Attributes { get; private set; }

        public Character(SerializeReader reader)
        {
            Token = reader.ReadUnicodeStatic(44);
            Username = reader.ReadUnicodeStatic(13);
            Pos = reader.ReadVec3();
            Dir = reader.ReadVec3();
            Attributes = new Attributes(reader);
        }
    }

}