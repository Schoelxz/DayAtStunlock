﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorGradientLerp : MonoBehaviour
{
    Gradient g;

    Color myColor = Color.green;

    [Range(0.1f, 0.9f)]
    public float gradientTweaker = 0.5f;

    public Slider mySlider;
    public Image myImage;
    
    void Start ()
    {
        GradientColorKey[] gck;
        g = new Gradient();
        gck = new GradientColorKey[3];
        gck[0].color = Color.red;
        gck[0].time = 0.0F;
        gck[1].color = Color.yellow;
        gck[1].time = gradientTweaker;
        gck[2].color = Color.green;
        gck[2].time = 1.0F;

        GradientAlphaKey[] gak;
        gak = new GradientAlphaKey[2];
        gak[0].alpha = 1.0F;
        gak[0].time = 0.0F;
        gak[1].alpha = 1.0F;
        gak[1].time = 1.0F;

        g.SetKeys(gck, gak);
    }

    void Update ()
    {
        float value = mySlider.value;

        myImage.color = g.Evaluate(value);
    }
}
