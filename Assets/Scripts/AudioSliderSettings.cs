using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderSettings : MonoBehaviour {

	private static readonly string BackgroundPref = "BackgroundPref";
	private static readonly string SFXPref = "SFXPref";
	//private int firstPlayInt;
	public Slider backgroundSlider, sfxSlider;
	private float backgroundfloat, sfxfloat;
	public Text text;


	// Use this for initialization
	void Start () {
		backgroundfloat = PlayerPrefs.GetFloat (BackgroundPref);
		sfxfloat = PlayerPrefs.GetFloat (SFXPref);

		backgroundSlider.value = backgroundfloat;
		sfxSlider.value = sfxfloat;

		text.text = "AudioManager Volume =" + AudioManager.instance.MusicVolume.ToString() + "\n" + "SFX Volume =" + AudioManager.instance.SfxVolume.ToString() + "\n" +
			"Settings Volume =" + backgroundSlider.value.ToString() + "\n" + "Settings SFX =" + sfxSlider.value.ToString();


	}
	
	public void onValueChangedBGM(float value){
		backgroundfloat = backgroundSlider.value;
		PlayerPrefs.SetFloat (BackgroundPref, backgroundfloat);
		AudioManager.instance.SetMusicVolume(backgroundfloat);

		text.text = "AudioManager Volume =" + AudioManager.instance.MusicVolume.ToString() + "\n" + "SFX Volume =" + AudioManager.instance.SfxVolume.ToString() + "\n" +
			"Settings Volume =" + backgroundSlider.value.ToString() + "\n" + "Settings SFX =" + sfxSlider.value.ToString();
	}

	public void onValueChangedSFX(float value){
		sfxfloat = sfxSlider.value;
		PlayerPrefs.SetFloat (SFXPref, sfxfloat);
		AudioManager.instance.SetSfxVolume(sfxfloat);

		text.text = "AudioManager Volume =" + AudioManager.instance.MusicVolume.ToString() + "\n" + "SFX Volume =" + AudioManager.instance.SfxVolume.ToString() + "\n" +
			"Settings Volume =" + backgroundSlider.value.ToString() + "\n" + "Settings SFX =" + sfxSlider.value.ToString();
	}

	public void saveAudioSettings(){
		PlayerPrefs.SetFloat (BackgroundPref, backgroundfloat);
		PlayerPrefs.SetFloat (SFXPref, sfxfloat);

		text.text = "AudioManager Volume =" + AudioManager.instance.MusicVolume.ToString() + "\n" + "SFX Volume =" + AudioManager.instance.SfxVolume.ToString() + "\n" +
			"Settings Volume =" + backgroundSlider.value.ToString() + "\n" + "Settings SFX =" + sfxSlider.value.ToString();
	}

}
