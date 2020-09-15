using System.Collections;
using System.Collections.Generic;
using Game_Server.Util;
using System;

namespace Leaderboards
{
    public class HallOfFameEntryData
    {
        //a single entry
        public string entryUsername { get; private set; }
        public DateTime entryDateTime { get; private set; }

        public HallOfFameEntryData(SerializeReader reader)
        {
            entryUsername = reader.ReadUnicodeStatic(13);
            entryDateTime = new DateTime(reader.ReadInt64());
        }
    }
}