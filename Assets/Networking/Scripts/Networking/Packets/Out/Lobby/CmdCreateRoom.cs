using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdCreateRoom : OutPacket
    {
        public string Token;
        public string RoomName;
        public bool IsLocked;
        public string Password;
        public int NumTurns;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdCreateRoom);
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Token, 44);
                    sw.WriteTextStatic(RoomName, 40);
                    sw.Write(IsLocked);
                    if(IsLocked){
                        sw.WriteTextStatic(Password, 40);
                    }
                    
                    sw.Write((byte)NumTurns);
                }
                return ms.ToArray();
            }
        }

    }
}
