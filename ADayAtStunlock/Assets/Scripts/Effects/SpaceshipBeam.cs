using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipBeam : MonoBehaviour
{
    private ParticleSystem myParticleSystem;
    private ParticleSystem.MainModule particleSettings;

    private void Awake()
    {
        myParticleSystem = GetComponent<ParticleSystem>();

        particleSettings = myParticleSystem.main;

        particleSettings.duration = SpaceshipMovement.myInstance.pauseTime;
    }

    void Start ()
    {
        myParticleSystem.Play();
    }

    private void Update()
    {
        if (!myParticleSystem.IsAlive())
            Destroy(gameObject);
    }
}
