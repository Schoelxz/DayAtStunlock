using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [Tooltip("2D -> 3D Sound")]
    [Range(0f, 1f)]
    public float spatialBlend;

    //Not currently in use
    //public enum SoundType { music, soundEffect, test };
    //public SoundType soundType;

    [HideInInspector]
    public AudioSource source;


}
