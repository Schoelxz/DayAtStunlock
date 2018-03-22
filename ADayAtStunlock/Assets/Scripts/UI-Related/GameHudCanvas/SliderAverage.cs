using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAverage : MonoBehaviour
{
    public Slider happySlider, motivationSlider;

	// Update is called once per frame
	void Update ()
    {
        happySlider.value = DAS.NPC.s_happyAverage;
        motivationSlider.value = DAS.NPC.s_motivationAverage;
    }
}
