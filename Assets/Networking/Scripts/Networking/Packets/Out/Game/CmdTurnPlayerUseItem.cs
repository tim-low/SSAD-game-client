using System;
using System.IO;

using Game_Server.Util;
using SuperSad.Model;
using SuperSad.Gameplay;

namespace SuperSad.Networking
{
    public class CmdTurnPlayerUseItem : OutPacket
    {
        public ItemType ItemIdentifier;
        public string Victim;
        public BoardTile Tile;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CmdTurnPlayerUseItem);
        }

        public override int ExpectedSize()
        {
            return 83;
        }

        public override byte[] GetBytes()
        {
            using(var ms = new MemoryStream())
            {
                using (var sw = new SerializeWriter(ms))
                {
                    sw.Write((int)ItemIdentifier);
                    sw.WriteTextStatic(Victim, 44);
                    sw.Write(Tile);
                }
                return ms.ToArray();
            }
        }

    }
}