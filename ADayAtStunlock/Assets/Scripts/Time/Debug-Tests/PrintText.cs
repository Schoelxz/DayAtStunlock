using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintText : MonoBehaviour {

    List<Text> childrenText = new List<Text>();

	// Use this for initialization
	void Start ()
    {
        foreach (Transform child in transform)
        {
           childrenText.Add(child.GetComponent<Text>());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < childrenText.Count; i++)
        {
            if (i == 0)
                childrenText[i].text = TimeSystem.TimePassedSeconds.ToString("0.00000000") + " seconds";
            else if (i == 1)
                childrenText[i].text = TimeSystem.TimePassedMinutes.ToString("0.00000000") + " minutes";
            else if (i == 2)
                childrenText[i].text = TimeSystem.TimePassedHours.ToString("0.00000000") + " hours";
            else if (i == 3)
                childrenText[i].text = TimeSystem.RealTimePassedSeconds.ToString("0.00000000") + " real seconds";
            else if (i == 4)
                childrenText[i].text = TimeSystem.TimeMultiplier.ToString("0.00000000") + " time multiplier";
            else if (i == 5)
                childrenText[i].text = TimeSystem.DeltaTime.ToString("0.00000000") + " game delta time";
            else if (i == 6)
                childrenText[i].text = Time.deltaTime.ToString("0.00000000") + " real delta time";
        }



    }
}
