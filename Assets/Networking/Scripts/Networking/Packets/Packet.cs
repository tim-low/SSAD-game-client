using System;
using System.IO;
using Game_Server.Util;

namespace SuperSad.Networking
{
    /// <summary>
    /// Base class for the packet 
    /// </summary>
    public class Packet
    {
        public readonly byte[] Buffer;
        public readonly ushort Id;


        /*
        private SerializeReader reader;
        public SerializeReader Reader
        {
            get
            {
                if (DoneReading)
                {
                    reader = new SerializeReader(new MemoryStream(Buffer));
                    DoneReading = false;
                }
                return reader;
            }
        }
        */

        public readonly SerializeWriter Writer;
        public readonly GameClient Sender;

        public Packet(ushort id)
        {
            Writer = new SerializeWriter(new MemoryStream());
            Id = id;

            Writer.Write(id);
        }

        public Packet(GameClient sender, ushort id, byte[] buffer)
        {
            Sender = sender;
            Buffer = buffer;
            Id = id;
            //reader = new SerializeReader(new MemoryStream(Buffer));
        }

        public void SendBack(Packet packet)
        {
            Sender.Send(packet, true);
        }

        /*public void SendBackError(string format, params object[] args)
        {
            Sender.SendError(format, args);
        }*/
    }

    /// <summary>
    /// Used for Packet Handling & Reflection
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class PacketAttribute : Attribute
    {
        public PacketAttribute(ushort id)
        {
            Id = id;
        }

        public ushort Id { get; }
    }
}
