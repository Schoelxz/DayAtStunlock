using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameClock : MonoBehaviour {
    Text timeText;
    float previousTimeMultiplier;
    private void Start()
    {
        timeText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        timeText.text = "Time: " + (int)DAS.TimeSystem.TimePassedSeconds;
	}

    private void ResetTime()
    {

    }
    private void ToggleFreezeTime()
    {
        
        if (DAS.TimeSystem.TimeMultiplier >= 0)
        {
            previousTimeMultiplier = DAS.TimeSystem.TimeMultiplier;
            DAS.TimeSystem.TimeMultiplier = 0;
        }
        else
        {
            DAS.TimeSystem.TimeMultiplier = previousTimeMultiplier;
        }
    }
}
