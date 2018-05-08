using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {
    
    [Range(0f, 1f)]
    static public float masterVolume = 1f;
    public List<AudioHelper> listOfAudioHelpers = new List<AudioHelper>();
    //public ArrayList listOfAudioSourcesInScene;
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


        //      foreach (Sound s in sounds)
        //      {
        //          s.source = gameObject.AddComponent<AudioSource>();
        //          s.source.clip   = s.clip;
        //          s.source.volume = s.volume * masterVolume;
        //          s.source.pitch  = s.pitch;
        //          s.source.loop   = s.loop;
        //          s.source.spatialBlend = s.spatialBlend;
        //      }

        listOfAudioHelpers.Clear();

    }

    void Start()
    {
        PlaySound("Theme", gameObject);
    }

    public void PlaySound(string name, GameObject self) {
        if(self.GetComponent<AudioHelper>() == null)
        {
            self.AddComponent<AudioHelper>();
        }
        AudioHelper myHelper = self.GetComponent<AudioHelper>();
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sounds: " + name + "not found!");
            return;
        }

        AudioSource source = null;
        //Check for an audiosource, if it has none, add and play the one that was requested.
        if (myHelper.listOfAudioSourcesOnObject == null)
        {
            source = myHelper.CreateNewAudioSource();
        }
        else if (myHelper.FindAudioSourceByName(name) == null)
        {
            source = myHelper.CreateNewAudioSource();
        }
        else
        {
            source = myHelper.FindAudioSourceByName(name);
        }
        source.clip = s.clip;
        source.volume = s.volume * masterVolume;
        source.pitch = s.pitch;
        source.loop = s.loop;
        source.spatialBlend = s.spatialBlend;
        source.Play();
	}

    public void StopSound(string name, GameObject self)
    {
        if (self.GetComponent<AudioHelper>() == null)
        {
            Debug.LogWarning("Warning, couldn't find audioHelper in " + self + ", can't stop it");
            return;
        }
        AudioHelper myHelper = self.GetComponent<AudioHelper>();
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds: " + name + "not found!");
            return;
        }
        if (myHelper.FindAudioSourceByName(name) == null)
        {
            Debug.LogWarning("Correct AudioSource not found!");
            return;
        }
            myHelper.FindAudioSourceByName(name).Stop();

    }
    public bool isPlaying(string name, GameObject self)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds: " + name + "not found!");
            return false;
        }

        bool response = self.GetComponent<AudioHelper>().FindAudioSourceByName(name).isPlaying;
        
        return response;
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
    public void AddToHelperList(AudioHelper targetToAdd)
    {
        listOfAudioHelpers.Add(targetToAdd);
    }
}
