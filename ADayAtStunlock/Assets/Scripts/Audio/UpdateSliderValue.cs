using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderValue : MonoBehaviour {

    Slider audioSlider;
    Text audioVolumeText;

    private void Start()
    {
        audioSlider = GetComponent<Slider>();
        audioVolumeText = GetComponentInChildren<Text>();
    }
    // Update is called once per frame
    void Update () {
        audioVolumeText.text = (AudioManager.instance.GetMasterVolume() * 100).ToString();

    }
}
