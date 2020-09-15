using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySfx1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string bgm = PlayerPrefs.GetString ("BGM");
		if (bgm != "BGM2") {
			AudioManager.instance.PlayMusic ("BGM2");
			PlayerPrefs.SetString ("BGM", "BGM2");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
