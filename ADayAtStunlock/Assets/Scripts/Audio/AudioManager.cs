using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [Range(0f, 1f)]
    static public float masterVolume = 1f;
    [Range(0f, 1f)]
    static public float volumeSound = 1f;
    [Range(0f, 1f)]
    static  public float volumeMusic = 1f;

    public Sound[] sounds;
    public static AudioManager instance;

	void Awake () {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);


		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            switch(s.audioType)
            {
                case Sound.AudioType.music:
                    s.source.volume = s.volume * volumeMusic * masterVolume;
                break;

                case Sound.AudioType.soundEffect:
                    s.source.volume = s.volume * volumeSound * masterVolume;
                break;
                default:
                    s.source.volume = s.volume * masterVolume;
                    Debug.LogWarning("Unrecognized audioType" + s.audioType);
                    break;
            }
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
	}
    void Start()
    {
        Play("Theme");
    }
    public void Play(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds: " + name + "not found!");
            return;
        }
            
        s.source.Play();
	}
}
