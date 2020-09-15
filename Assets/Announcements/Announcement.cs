using System;
using UnityEngine;
using Game_Server.Util;

namespace SuperSad.GameEvents
{
    public class Announcement
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Texture2D BannerTexture { get; private set; }

        public Announcement(SerializeReader reader)
        {
            // Read Notice info
            Id = reader.ReadInt32();
            Name = reader.ReadUnicode();
            Description = reader.ReadUnicode();
            
            // Read Banner Image
            int byteLength = reader.ReadInt32();
            byte[] image = reader.ReadBytes(byteLength);
            BannerTexture = ConvertToTexture(image);
        }

        private static Texture2D ConvertToTexture(byte[] bytes)
        {
            string imageStr = Convert.ToBase64String(bytes); //download is the WWW object
            //PlayerPrefs.SetString("SecondaryImage_ByteString", imageStr);

            Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            texture.LoadImage(Convert.FromBase64String(imageStr));//PlayerPrefs.GetString("PrimaryImage_ByteString")));

            return texture;
        }
    }

}