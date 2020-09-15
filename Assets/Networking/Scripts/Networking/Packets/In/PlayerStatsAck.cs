using System;
using System.IO;
//using SampleClient.Network.Util;

using Game_Server.Util;

namespace SuperSad.Networking
{
    public class PlayerStatsAck : InPacket
    {
        // Might want to split them up further into like Experience object and Stats object.
        // Since not very maintainable
        public int RequirementsExp {get; private set;}
        public int DesignExp { get; private set; }
        public int ImplementationExp { get; private set; }
        public int TestingExp { get; private set; }
        public int DeploymentExp { get; private set; }
        public int MaintenanceExp { get; private set; }
        public int RequirementsCorrect { get; private set; }
        public int RequirementsTotalQns { get; private set; }
        public int DesignCorrect { get; private set; }
        public int DesignTotalQns { get; private set; }
        public int ImplementationCorrect { get; private set; }

        public int ImplementationTotalQns { get; private set; }
        public int TestingCorrect { get; private set; }

        public int TestingTotalQns { get; private set; }
        public int DeploymentCorrect { get; private set; }
        public int DeploymentTotalQns { get; private set; }
        public int MaintenanceCorrect { get; private set; }
        public int MaintenanceTotalQns { get; private set; }

        public int GameCount {get; private set; }

        public PlayerStatsAck(Packet packet) : base(packet)
        {
            RequirementsExp = Reader.ReadInt32();
            DesignExp = Reader.ReadInt32();
            ImplementationExp = Reader.ReadInt32();
            TestingExp = Reader.ReadInt32();
            DeploymentExp = Reader.ReadInt32();
            MaintenanceExp = Reader.ReadInt32();
            RequirementsCorrect = Reader.ReadInt32();
            RequirementsTotalQns = Reader.ReadInt32();
            
            DesignCorrect = Reader.ReadInt32();
            DesignTotalQns = Reader.ReadInt32();
            
            ImplementationCorrect = Reader.ReadInt32();
            ImplementationTotalQns = Reader.ReadInt32();

            TestingCorrect = Reader.ReadInt32();
            TestingTotalQns = Reader.ReadInt32();

            DeploymentCorrect = Reader.ReadInt32();
            DeploymentTotalQns = Reader.ReadInt32();

            MaintenanceCorrect = Reader.ReadInt32();
            MaintenanceTotalQns = Reader.ReadInt32();

            GameCount = Reader.ReadInt32();
        }
    }
}
