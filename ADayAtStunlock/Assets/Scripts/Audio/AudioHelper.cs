using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHelper : MonoBehaviour {
    //AudioManager audioManager;
    public List<AudioSource> listOfAudioSourcesOnObject = new List<AudioSource>();

	// Use this for initialization
	void Start ()
    {
        listOfAudioSourcesOnObject.Clear();
        //audioManager = FindObjectOfType<AudioManager>();

    }

    public AudioSource FindAudioSourceByName(string clipName)
    {
        if (listOfAudioSourcesOnObject == null)
            return null;
        foreach (AudioSource source in listOfAudioSourcesOnObject)
        {
            if(source.clip.name == clipName)
            {
                return source;
            }
        }
        return null;
    }
    public AudioSource CreateNewAudioSource()
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        AddAudioSourceToList(newAudio);
        return newAudio;
    }
    private void AddAudioSourceToList(AudioSource newAudioSource)
    {
        listOfAudioSourcesOnObject.Add(newAudioSource);
    }
}
