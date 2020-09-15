using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct SetAudioClip
{
    public string name;
    public AudioClip audioClip;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource SFXPrefab;   // to be used for instantiating a new AudioSource
    [SerializeField]
    private AudioSource musicPlayerPrefab;  // to instantiate conveniently in inspector
    [SerializeField]
    private AudioSource SpatialSFXPrefab;   // to be used for instantiating a new spatialised AudioSource

    public static AudioManager instance = null;
    private AudioSource musicPlayer;    // for reference

    // music clips
    [SerializeField]
    private SetAudioClip[] musicClipsSerialize;  // to serialize in inspector
    static Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>(); // to access conveniently via name

    // SFX clips
    [SerializeField]
    private SetAudioClip[] SFXClipsSerialize;       // to serialize in inspector
    private static Dictionary<string, AudioClip> SFXClips = new Dictionary<string, AudioClip>();   // to access conveniently via name
    
    private static string currentBGMName = "";  // name of currently playing BGM

    private static string currentSFXName = "";  // name of previously played SFX

    public float MusicVolume { get; private set; }
    public void SetMusicVolume(float newVolume)
    {
        MusicVolume = newVolume;
        musicPlayer.volume = MusicVolume;
    }
    public float SfxVolume { get; private set; }
    public void SetSfxVolume(float newVolume)
    {
        SfxVolume = newVolume;
    }

    private void Awake()
    {
        if (instance != null)   // instance of AudioManager already exists
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        // Set Volume
        MusicVolume = 0.3f;
        SfxVolume = 0.3f;

        // add music clips to dictionary
        for (int i = 0; i < musicClipsSerialize.Length; ++i)
        {
            if (musicClips.ContainsKey(musicClipsSerialize[i].name))
                Debug.Log("AudioManager BGM already contains " + musicClipsSerialize[i].name);
            else
                musicClips.Add(musicClipsSerialize[i].name, musicClipsSerialize[i].audioClip);
        }
        // add SFX clips to dictionary
        for (int i = 0; i < SFXClipsSerialize.Length; ++i)
        {
            if (SFXClips.ContainsKey(SFXClipsSerialize[i].name))
                Debug.Log("AudioManager SFX already contains " + SFXClipsSerialize[i].name);
            else
                SFXClips.Add(SFXClipsSerialize[i].name, SFXClipsSerialize[i].audioClip);
        }

        // create BGM player
        musicPlayer = Instantiate(musicPlayerPrefab);
        musicPlayer.transform.SetParent(this.transform);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        // can iterate through GO children of this AudioManager here to do things
        // the GO children will be temporary SFX AudioSources

        if (currentSFXName != "")
            currentSFXName = "";
    }

    // generic
    public void PlayMusic(string key)
    {
        if (key == currentBGMName)  // this BGM is already playing
            return;

        if (musicClips.ContainsKey(key))
        {
            musicPlayer.clip = musicClips[key];
            musicPlayer.Play();
            currentBGMName = key;
        }
    }

    public void PlaySpatialSFX(string key, Vector3 position)
    {
        // change to instantiate SFX Prefab!

        if (key == currentSFXName)
            currentSFXName = key;

        if (SFXClips.ContainsKey(key))
        {
            // create a temp AudioSource GameObject; it'll destroy itself after its length
            AudioSource SFXAudioSource = Instantiate(SpatialSFXPrefab);
            SFXAudioSource.clip = SFXClips[key];
            SFXAudioSource.volume = SfxVolume;
            SFXAudioSource.Play();

            // Set destruction only after SFX finishes playing
            DontDestroyOnLoad(SFXAudioSource.gameObject);
            Destroy(SFXAudioSource.gameObject, SFXAudioSource.clip.length);

            // set position
            SFXAudioSource.transform.position = position;

            currentSFXName = key;
        }
    }
    
    public void PlaySFX(string key)
    {
        if (key == currentSFXName)
            currentSFXName = key;

        if (SFXClips.ContainsKey(key))
        {
            // create a temp AudioSource GameObject; it'll destroy itself after its length
            AudioSource SFXAudioSource = Instantiate(SFXPrefab);
            SFXAudioSource.clip = SFXClips[key];
            SFXAudioSource.volume = SfxVolume;
            SFXAudioSource.Play();

            // Set destruction only after SFX finishes playing
            DontDestroyOnLoad(SFXAudioSource.gameObject);
            Destroy(SFXAudioSource.gameObject, SFXAudioSource.clip.length);

            // newGO.volume = SettingsData.GetSFXVolumeRange();
            // newGO.Play();
            // Destroy(newGO, clip.Length);

            //return SFXAudioSource.clip.length;

            currentSFXName = key;
        }

        //return 0f;
    }
    
}
