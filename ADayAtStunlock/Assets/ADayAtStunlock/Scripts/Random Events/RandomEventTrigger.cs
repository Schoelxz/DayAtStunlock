using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour {

    List<System.Action> randomEvents = new List<System.Action>(); //Add random event functions here
    static public bool stopWork;
    

	// Use this for initialization
	void Start () {
        stopWork = false;
        InvokeRepeating("TriggerRandomEvent", 5, 60);
        randomEvents.Add(TrainEvent);
	}
	
	// Update is called once per frame
	void Update () {
        
	}


    void TriggerRandomEvent()
    {
        stopWork = true;

        randomEvents[Random.Range(0, randomEvents.Count)]();
    }


    void TrainEvent()
    {
        ScreenShake.shakeDuration = 5;
    }
}
