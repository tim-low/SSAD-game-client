using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarDrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AudioManager.instance.PlaySpatialSFX ("PillarDrop", transform.position);
		Debug.Log ("Playing Pillar Drop Sfx");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
