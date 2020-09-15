using System;
using System.IO;
//using System.Numerics;
//using SampleClient.Network.Util;
using Game_Server.Util;
using UnityEngine;

namespace SuperSad.Networking
{
    public class CmdMovePlayer : OutPacket
    {

        public string Token;
        public Vector3 StartPos;
        public Vector3 TargetPos;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdMovePlayer);
        }

        public override int ExpectedSize() { return 64; }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
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
