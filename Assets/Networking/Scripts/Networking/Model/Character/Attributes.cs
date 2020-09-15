using Game_Server.Model;
using Game_Server.Util;

namespace SuperSad.Model
{
    public class Attributes {

        public enum Parts
        {
            Head,
            Top,
            Bottom,
            Shoes
        }

        public int Head { get; private set; }
        public int Top { get; private set; }
        public int Bottom { get; private set; }
        public int Shoes { get; private set; }

        public Attributes(SerializeReader reader)
        {
            Head = reader.ReadByte();
            Top = reader.ReadByte();
            Bottom = reader.ReadByte();
            Shoes = reader.ReadByte();

        }
    }

}