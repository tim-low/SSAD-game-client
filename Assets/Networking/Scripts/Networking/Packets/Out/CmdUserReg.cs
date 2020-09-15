using System;
using System.IO;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class CmdUserReg : OutPacket
    {
        public string Username;
        public string Password;
        public string Email;
        public string StudentName;
        public string Class;
        public int Semester;
        public int Year;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdUserReg);
        }

        public override int ExpectedSize()
        {
            return 13+40+4+Email.Length + Class.Length + 4 + 4;
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.WriteTextStatic(Username, 13);
                    sw.WriteTextStatic(Password, 40);
                    sw.WriteText(Email, false);
                    sw.WriteText(StudentName, false);
                    sw.WriteTextStatic(Class, 4);
                    sw.Write(Semester);
                    sw.Write(Year);
                }
                return ms.ToArray();
            }
        }

    }
}
