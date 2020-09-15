using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable(){
		AudioManager.instance.PlaySFX ("UIPop");
	}
	
	// Update is called once per frame
	void Update () {
	}
}
