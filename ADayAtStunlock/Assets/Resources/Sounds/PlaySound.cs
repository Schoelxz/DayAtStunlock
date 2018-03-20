using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        //audio.Play(44100);
    }
}
