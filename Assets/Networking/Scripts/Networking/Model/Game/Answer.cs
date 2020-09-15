using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game_Server.Util;

namespace SuperSad.Model
{
    public class Answer {

        public int Id { get; private set; }
        public string Text { get; private set; }

        public Answer(SerializeReader reader)
        {
            Id = reader.ReadInt32();
            Debug.Log("Answer ID: " + Id);
            Text = reader.ReadUnicode();    //reader.ReadUnicodeStatic(40);
            Debug.Log("Answer: " + Text);
        }
    }

}