using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperSad.Networking {
    public class GetChestRewardAck : InPacket 
    {
	    public int AttributeNumber {get; private set;}
	    public int ItemNumber { get; private set; }

	    public GetChestRewardAck(Packet packet) : base(packet)
	    {
		    AttributeNumber = Reader.ReadInt32();
		    ItemNumber = Reader.ReadInt32();
	    }
	
    }
}
