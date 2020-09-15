using System;
//using System.Numerics;
using UnityEngine;

namespace SuperSad.Networking
{
    public class MovePlayerAck : InPacket
    {
        public readonly string Token;
        public readonly Vector3 EndPos;

        public MovePlayerAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
            EndPos = Reader.ReadVec3();
        }
    }
}
