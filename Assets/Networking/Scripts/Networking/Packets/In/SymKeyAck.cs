using System.Collections;
using System.Collections.Generic;

namespace SuperSad.Networking
{
    public class SymKeyAck : InPacket {

        public byte[] Key { get; private set; }
        public byte[] IV { get; private set; }

        private const int KeySize = 32;
        private const int IVSize = 16;

        public SymKeyAck(Packet packet) : base(packet)
        {
            Key = new byte[KeySize];
            for (int i = 0; i < KeySize; i++)
                Key[i] = Reader.ReadByte();

            IV = new byte[IVSize];
            for (int i = 0; i < IVSize; i++)
                IV[i] = Reader.ReadByte();
        }
    }

}