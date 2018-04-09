using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {
    
    [Range(0f, 1f)]
    static public float masterVolume = 1f;

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
            s.source.clip   = s.clip;
            s.source.volume = s.volume * masterVolume;
            s.source.pitch  = s.pitch;
            s.source.loop   = s.loop;
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

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds: " + name + "not found!");
            return;
        }

        s.source.Stop();

    }

    public void SetVolume(Slider slider)
    {
        masterVolume = slider.value;
        Text valueText = slider.gameObject.GetComponentInChildren<Text>();
        int volumeValue = (int)(slider.value * 100);
        valueText.text = volumeValue.ToString();
        UpdateSoundVolumeAll();

    }
    //public float GetVolume()
    //{
    //    return masterVolume;
    //}
    public void UpdateSoundVolumeAll()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * masterVolume;
        }
    }
}
