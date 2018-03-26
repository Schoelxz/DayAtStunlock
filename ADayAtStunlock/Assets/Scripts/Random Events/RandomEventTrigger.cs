using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour {

    List<System.Action> randomEvents = new List<System.Action>(); //Add random event functions here
    static public bool stopWork;
    List<float> motivationList = new List<float>();

	// Use this for initialization
	void Start () {
        stopWork = false;
        InvokeRepeating("TriggerRandomEvent", 60, 60);
        randomEvents.Add(TrainEvent);
	}
	
	// Update is called once per frame
	void Update () {
        
	}


    void TriggerRandomEvent()
    {

        randomEvents[Random.Range(0, randomEvents.Count)]();
    }


    void TrainEvent()
    {
        ScreenShake.shakeDuration = 5;

        foreach (var npc in DAS.NPC.s_npcList)
        {
            motivationList.Add(npc.myFeelings.Motivation);
            npc.myFeelings.Motivation = 0;
        }

        Invoke("ResetMotivation", 5);
    }

    void ResetMotivation()
    {
        for(int i = 0; i < DAS.NPC.s_npcList.Count; i++)
        {
            DAS.NPC.s_npcList[i].myFeelings.Motivation = motivationList[i];
        }
    }
}
