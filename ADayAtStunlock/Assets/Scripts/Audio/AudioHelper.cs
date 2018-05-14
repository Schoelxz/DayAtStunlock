using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioHelper : MonoBehaviour {

    public List<AudioSource> listOfAudioSourcesOnObject = new List<AudioSource>();

	void Start ()
    {
        //listOfAudioSourcesOnObject.Clear();
        //CreateNewAudioSource();
    }

    public AudioSource FindAudioSourceByName(string clipName)
    {
        if (listOfAudioSourcesOnObject.Count == 0)
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

    public AudioSource CreateNewAudioSource(Sound soundValues)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();

        newAudio.clip = soundValues.clip;
        newAudio.volume = soundValues.volume * AudioManager.masterVolume;
        newAudio.pitch = soundValues.pitch;
        newAudio.loop = soundValues.loop;
        newAudio.spatialBlend = soundValues.spatialBlend;

        AddAudioSourceToList(newAudio);
        return newAudio;
    }
    private void AddAudioSourceToList(AudioSource newAudioSource)
    {
        listOfAudioSourcesOnObject.Add(newAudioSource);
    }
}
