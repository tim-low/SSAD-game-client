using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginBGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string bgm = PlayerPrefs.GetString ("BGM");
		if (bgm != "BGM2") {
			AudioManager.instance.PlayMusic ("BGM2");
			PlayerPrefs.SetString ("BGM", "BGM2");
		}
		Debug.Log ("Login, Play BGM2");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
