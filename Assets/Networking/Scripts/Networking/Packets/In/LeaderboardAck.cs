using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leaderboards;

namespace SuperSad.Networking
{
    public class LeaderboardAck : InPacket {

        public int OwnRank { get; private set; }
        public bool IsLastPage { get; private set; }
        public LeaderboardEntryData[] Entries { get; private set; }

        public LeaderboardAck(Packet packet) : base(packet)
        {
            OwnRank = Reader.ReadInt32();
            IsLastPage = Reader.ReadBoolean();
            int entrySize = Reader.ReadInt32();
            Entries = new LeaderboardEntryData[entrySize];
            for (int i = 0; i < entrySize; i++)
            {
                Entries[i] = new LeaderboardEntryData(Reader);
            }
        }
    }

}