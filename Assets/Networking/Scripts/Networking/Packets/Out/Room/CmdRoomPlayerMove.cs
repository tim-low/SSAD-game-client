using System;
using System.IO;
//using SampleClient.Network.Util;
using UnityEngine;
using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdRoomPlayerMove : OutPacket
    {
        public string Token;
        public Vector3 StartPos;
        public Vector3 TargetPos;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdRoomPlayerMove);
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                    sw.Write(StartPos);
                    sw.Write(TargetPos);
                }
                return ms.ToArray();
            }
        }

    }
}
