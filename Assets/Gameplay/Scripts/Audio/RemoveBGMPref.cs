using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBGMPref : MonoBehaviour {

	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString ("BGM", "None");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
