using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSfx : MonoBehaviour {

	public AudioSource createSfx;
	public AudioClip clickcreateFx;

	public void ClickSound()
	{
		createSfx.PlayOneShot (clickcreateFx);	
	}

}
