using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour {
    
    [Range(0f, 1f)]
    static public float masterVolume = 1f;
    public List<AudioHelper> listOfAudioHelpers = new List<AudioHelper>();
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
        listOfAudioHelpers.Clear();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearAudioHelperList();
    }

    void Start()
    {
        PlaySound("Theme", gameObject);
    }



    public void PlaySound(string name, GameObject self) {

        ////
        //Getting Helper
        ////

        //Get the values of the requested sound effect
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //Debug.Log(self.name);
        if (s == null)
        {
            Debug.LogWarning("Sounds: " + name + "not found!");
            return;
        }

        //Check for existing audiohelper, if there is none, create one.
        
        if (self.GetComponent<AudioHelper>() == null)
        {
            CreateNewAudioHelper(self);
        }
        //Find the Audiohelper from the audiohelper list.
        AudioHelper myHelper = null;
        myHelper = FindAudioHelper(self.GetComponent<AudioHelper>());
        if(myHelper == null)
        {
            Debug.LogWarning("No helper found, something broke!");
            return;
        }

        ////
        //Getting AudioSource
        ////

        AudioSource source = null;
        //Check for an audiosource, if it has none, add and play the one that was requested.
        if (myHelper.FindAudioSourceByName(s.clip.name) == null)
        {
            source = myHelper.CreateNewAudioSource(s);
        }
        else
        {
            source = myHelper.FindAudioSourceByName(s.clip.name);
        }
        source.Play();
    }

    public void StopAllSoundEffects()
    {
        foreach (var helper in listOfAudioHelpers)
        {
            foreach (var sound in helper.listOfAudioSourcesOnObject)
            {
                if(sound.clip.name != "Backbay Lounge")
                {
                    sound.Stop();
                }
            }
        }
    }

    public void StopSound(string name, GameObject self)
    {
        //Get the values of the requested sound effect
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sounds: " + name + "not found!");
            return;
        }
        //Find the Audiohelper from the audiohelper list.
        AudioHelper myHelper = null;
        myHelper = FindAudioHelper(self.GetComponent<AudioHelper>());
        if (myHelper == null)
        {
            Debug.LogWarning("No helper found, something broke!");
            return;
        }
        
        if (myHelper.FindAudioSourceByName(s.clip.name) == null)
        {
            Debug.LogWarning("Correct AudioSource not found!");
            return;
        }
            myHelper.FindAudioSourceByName(s.clip.name).Stop();

    }
    private Sound FindSoundReference(string nameOfSound)
    {
        Sound s = Array.Find(sounds, sound => sound.name == nameOfSound);
        if (s == null)
        {
            Debug.LogWarning("Sounds: " + nameOfSound + "not found!");
            return null;
        }
        return s;
    }
    #region Broken
    //public bool DoIHaveAnAudioSource(string name, GameObject self)
    //{
    //    AudioHelper audioHelper = self.GetComponent<AudioHelper>();
    //    if (audioHelper == null)
    //        return false;
    //    if (audioHelper.FindAudioSourceByName(name) == null)
    //        return false;
    //    else
    //        return true;
    //}
    /*
    public bool IsSoundPlayingOnSelf(string name, GameObject self)
    {
        //Get the values of the requested sound effect or return if unavailable
        Sound sound = FindSoundReference(name);
        if(sound == null)
        {
            Debug.Log("Sound " + name + ", doesn't exist");
            return false;
        }
        //Find the Audiohelper from the audiohelper list.
        AudioHelper myHelper = null;
        myHelper = FindAudioHelper(self.GetComponent<AudioHelper>());
        if (myHelper == null)
        {
            Debug.LogWarning("Missing helper");
            return false;
        }

        if (myHelper.FindAudioSourceByName(sound.clip.name) == null)
        {
            Debug.LogWarning("Correct AudioSource not found!(In playingOnSelf)");
            return false;
        }
        return myHelper.FindAudioSourceByName(sound.clip.name).isPlaying;
    }
    */
    #endregion

    #region VolumeControl
    public void SetVolume(Slider slider)
    {
        masterVolume = slider.value;
        Text valueText = slider.gameObject.GetComponentInChildren<Text>();
        int volumeValue = (int)(slider.value * 100);
        valueText.text = volumeValue.ToString();
        UpdateSoundVolumeAll();

    }
    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public void UpdateSoundVolumeAll()
    {
        foreach (AudioHelper audioHelper in listOfAudioHelpers)
        {
            foreach(AudioSource soundSource in audioHelper.listOfAudioSourcesOnObject)
            {
                soundSource.volume = FindSoundVolumeByName(soundSource.clip.name) * masterVolume;
            }
        }
    }

    private float FindSoundVolumeByName(string compareName)
    {
        foreach (Sound sound in sounds)
        {
            if(sound.clip.name == compareName)
            {
                return sound.volume;
            }
        }
        return 0;
    }
    #endregion


    #region Helper

    public void CreateNewAudioHelper(GameObject targetInNeedOfHelper)
    {
        AudioHelper tempHelp = null;
        tempHelp = targetInNeedOfHelper.AddComponent<AudioHelper>();
        AddToHelperList(tempHelp);
    }

    private void AddToHelperList(AudioHelper targetToAdd)
    {
        listOfAudioHelpers.Add(targetToAdd);
    }

    private AudioHelper FindAudioHelper(AudioHelper findMe)
    {
        AudioHelper tempHelp = null;
        foreach (AudioHelper audioHelper in listOfAudioHelpers)
        {
            if (audioHelper == findMe)
            {
                tempHelp = audioHelper;
                return tempHelp;
            }
        }
        return null;
    }

    //Clear out missing helpers after transitioning into a different scene
    public void ClearAudioHelperList()
    {
        List<AudioHelper> temp = new List<AudioHelper>();
        foreach (AudioHelper helper in listOfAudioHelpers)
        {
            if (helper == null)
                temp.Add(helper);
        }
        foreach (var item in temp)
        {
            listOfAudioHelpers.Remove(item);
        }
    }
    #endregion
}
