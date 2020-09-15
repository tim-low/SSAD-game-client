using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Model;

namespace SuperSad.Networking
{
    public class StartQuizAck : InPacket
    {
        //public int Index {get; private set;}
        public string Question { get; private set; }
        public Answer[] Answers { get; private set; }
        public int Duration { get; private set; }

        public const int numAnswers = 4;

        public StartQuizAck(Packet packet) : base(packet)
        {
            //Index = Reader.ReadInt32();
            Question = Reader.ReadUnicode();    //Reader.ReadUnicodeStatic(40);

            Debug.Log("Question: " + Question);

            Answers = new Answer[numAnswers];
            for (int i = 0; i < numAnswers; i++)
            {
                Answers[i] = new Answer(Reader);
            }

            Duration = Reader.ReadInt32();
        }
    }

}