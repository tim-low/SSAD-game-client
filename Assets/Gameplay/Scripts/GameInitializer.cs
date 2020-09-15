using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SuperSad.Networking;

public class GameInitializer : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(1f);
        SendCmdInitializeGame();
    }
	
    private void SendCmdInitializeGame()
    {
        Packet cmd = new CmdInitializeGame
        {
            Token = UserState.Instance().Token
        }.CreatePacket();

        NetworkStreamManager.Instance.SendPacket(cmd);
    }
}
