using System;
using System.IO;
//using System.Numerics;
using System.Text;
using UnityEngine;

namespace Game_Server.Util
{
    public class SerializeWriter : BinaryWriter
    {
        private readonly Encoding _encoding = Encoding.Unicode;

        public SerializeWriter(Stream stream) : base(stream, Encoding.Unicode)
        {
        }

        public SerializeWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        {
            _encoding = encoding;
        }

        public byte[] GetBuffer()
        {
            MemoryStream baseStream = (MemoryStream)BaseStream;
            if (baseStream != null)
                return baseStream.ToArray();

            return null;
            //return (BaseStream as MemoryStream)?.ToArray();
        }

        public void Write(ISerializable model)
        {
            model.Serialize(this);
        }

        public void Write(Vector3 vec3)
        {
            Write(vec3.x);
            Write(vec3.y);
            Write(vec3.z);
        }

        /// <summary>
        /// Write a string to the underlying buffer with the default encoding which is Unicode
        /// </summary>
        /// <param name="text">the text to be send over the stream</param>
        /// <param name="prefixLength">to indicate if the packet contain the str length for processing</param>
        public void WriteText(string text, bool prefixLength = true)
        {
            if (text == null)
                text = "";

            var buf = _encoding.GetBytes(text + "\0");

            if (prefixLength)
                Write((ushort)text.Length);

            Write(buf);
        }

        /// <summary>
        /// Write a string with a prefixed max length to the underlying buffer with the default encoding which is Unicode
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <param name="nullTerminated"></param>
        public void WriteTextStatic(string str, int maxLength, bool nullTerminated = false)
        {
            if (str == null)
                str = "";

            if (str.Length > maxLength)
                str.Substring(0, maxLength);

            if(nullTerminated)
            {
                if (str.Length > maxLength - 1)
                    str = str.Substring(0, maxLength - 1);
                str += "\0";
            }

            var tempBuf = _encoding.GetBytes(str);

            var buf = new byte[maxLength * 2];
            Array.Copy(tempBuf, 0, buf, 0, tempBuf.Length);

            Write(buf);

        }
    }
}
