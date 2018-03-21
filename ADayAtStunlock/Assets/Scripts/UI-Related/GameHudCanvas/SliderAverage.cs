using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderAverage : MonoBehaviour
{
    public Slider happySlider, motivationSlider;

    float happyAverage, motivationAverage;

	// Update is called once per frame
	void Update ()
    {
        happyAverage = 0;
        motivationAverage = 0;

        foreach (var npc in DAS.NPC.s_npcList)
        {
            happyAverage += npc.myFeelings.Happiness;
            motivationAverage += npc.myFeelings.Motivation;
        }
        happyAverage /= DAS.NPC.s_npcList.Count;
        motivationAverage /= DAS.NPC.s_npcList.Count;

        happySlider.value = happyAverage;
        motivationSlider.value = motivationAverage;
    }
}
