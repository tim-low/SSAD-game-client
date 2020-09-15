using System;

using Game_Server.Model;
using SuperSad.Model;
using UnityEngine;

namespace SuperSad.Networking
{
    public class RoomPlayerMoveAck : InPacket
    {
        public string Token {get; private set;}
        public Vector3 StartPos {get; private set;}
        public Vector3 TargetPos {get; private set;}

        public RoomPlayerMoveAck(Packet packet) : base(packet)
        {
            Token = Reader.ReadUnicodeStatic(44);
            StartPos = Reader.ReadVec3();
            TargetPos = Reader.ReadVec3();
        }
    }
}
