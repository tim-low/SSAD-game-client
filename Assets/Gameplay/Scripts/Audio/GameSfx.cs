using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSfx : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string bgm = PlayerPrefs.GetString ("BGM");
		if(bgm != "BGM1"){
			AudioManager.instance.PlayMusic ("BGM1");
			PlayerPrefs.SetString("BGM","BGM1");
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
