using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioSlider : MonoBehaviour {

	//Sliders
	[SerializeField]
	private Slider BGMSlider;
	[SerializeField]
	private Slider SFXSlider;

	//Prefs
	private static readonly string bgmPref = "BGMPref";
	private static readonly string sfxPref = "SFXPref";
	//private static readonly string FirstPlay = "FirstPlay";

	//Values
	//private int firstPlayInt;
	private float bgmfloat;
	private float sfxfloat;

	// Use this for initialization
	void OnEnable () {
		bgmfloat = AudioManager.instance.MusicVolume;
		sfxfloat = AudioManager.instance.SfxVolume;

		BGMSlider.value = bgmfloat;
		SFXSlider.value = sfxfloat;
	}
	
	public void onValueChangeBGM(float value){
		bgmfloat = BGMSlider.value;
		AudioManager.instance.SetMusicVolume(bgmfloat);
		PlayerPrefs.SetFloat (bgmPref,bgmfloat );
		Debug.Log("Audio Manager set at" + SceneManager.GetActiveScene().name);
		Debug.Log ("BGM Volume = " + bgmfloat);

	}

	public void onValueChangeSFX(float value){
		sfxfloat = SFXSlider.value;
		AudioManager.instance.SetSfxVolume(sfxfloat);
		PlayerPrefs.SetFloat (sfxPref, sfxfloat );
		Debug.Log("Audio Manager set at" + SceneManager.GetActiveScene().name);
		Debug.Log ("SFX and SPA Volume = " + sfxfloat);
	}
		
	public void SaveSoundSettings(){
		PlayerPrefs.SetFloat (bgmPref,bgmfloat );
		PlayerPrefs.SetFloat (sfxPref, sfxfloat );
		Debug.Log ("Saving into PlayPref : bgm " + bgmfloat + "sfx " + sfxfloat);
		Debug.Log("PlayPref Values currently: " + PlayerPrefs.GetFloat(bgmPref) + " " + PlayerPrefs.GetFloat(sfxPref));
	}
		
}
