using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class GetUnlocksAck : InPacket
    {
        public byte Head {get; private set;}
        public byte Shirt {get; private set;}
        public byte Pants {get; private set;}
        public byte Shoes {get; private set;}

        public GetUnlocksAck(Packet packet) : base(packet)
        {
            Head = Reader.ReadByte();
            Shirt = Reader.ReadByte();
            Pants = Reader.ReadByte();
            Shoes = Reader.ReadByte();
        }
    }
}
