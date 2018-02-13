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
                childrenText[i].text = TimeSystem.TimePassedSeconds + " seconds";
            else if (i == 1)
                childrenText[i].text = TimeSystem.TimePassedMinutes + " minutes";
            else if (i == 2)
                childrenText[i].text = TimeSystem.TimePassedHours + " hours";
            else if (i == 3)
                childrenText[i].text = TimeSystem.RealTimePassedSeconds + " real seconds";
            else if (i == 4)
                childrenText[i].text = TimeSystem.TimeMultiplier + " time multiplier";
            else if (i == 5)
                childrenText[i].text = TimeSystem.DeltaTime + " game delta time";
            else if (i == 6)
                childrenText[i].text = Time.deltaTime + " real delta time";
        }
    }
}
