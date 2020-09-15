using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leaderboards;
using System;

namespace SuperSad.Networking
{
    public class HallOfFameAck : InPacket
    {
        
        //public String Username { get; private set; }
        public bool IsLastPage { get; private set; }
        public HallOfFameEntryData[] Entries { get; private set; }

        public HallOfFameAck(Packet packet) : base(packet)
        {
            IsLastPage = Reader.ReadBoolean();
            int entrySize = Reader.ReadInt32();

            Entries = new HallOfFameEntryData[entrySize];
            for (int i = 0; i < entrySize; i++)
            {
                Entries[i] = new HallOfFameEntryData(Reader);
            }

        }
    }
}
