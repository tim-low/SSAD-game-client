using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSettings : MonoBehaviour {

	//Sliders
	//public Slider BGMSlider;
	//public Slider SFXSlider;

	//Prefs
	private static readonly string bgmPref = "BGMPref";
	private static readonly string sfxPref = "SFXPref";
	//private static readonly string FirstPlay = "FirstPlay";

	//Values
	//private int firstPlayInt;
	private float bgmfloat;
	private float sfxfloat;

	// Use this for initialization
	void Start () {

        /*firstPlayInt = PlayerPrefs.GetInt (FirstPlay);

		if (firstPlayInt == 0) 
		{
			Debug.Log ("First Play");
			//First Launch
			bgmfloat = 0.25f;
			sfxfloat = 0.25f;

			//Assign initial values
			//BGMSlider.value = bgmfloat;
			//SFXSlider.value = sfxfloat;

			//Set into AudioManager
			updateSound(bgmfloat, sfxfloat);

			//Save Pref
			PlayerPrefs.SetFloat (bgmPref,bgmfloat );
			PlayerPrefs.SetFloat (sfxPref, sfxfloat );
			PlayerPrefs.SetInt (FirstPlay, -1);
		}*/
        //else {
        //Debug.Log("Not First Play");
        //Get values on launch
        bgmfloat = PlayerPrefs.GetFloat(bgmPref, 0.3f);
        sfxfloat = PlayerPrefs.GetFloat(sfxPref, 0.3f);

        //Set into sliders
        //BGMSlider.value = bgmfloat;
        //SFXSlider.value = sfxfloat;

        //Set into AudioManager
        updateSound(bgmfloat, sfxfloat);
        //}
    }

	public void updateSound(float bgm, float sfx){
		AudioManager.instance.SetMusicVolume(bgm);
		AudioManager.instance.SetSfxVolume(sfx);
		Debug.Log("Audio Manager set at" + SceneManager.GetActiveScene().name);
		Debug.Log ("BGM Volume = " + bgm);
		Debug.Log ("SFX and SPA Volume = " + sfx);
	}

}
