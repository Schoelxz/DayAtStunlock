using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour {

    List<System.Action> randomEvents = new List<System.Action>(); //Add random event functions here
    static public bool stopWork;
    List<float> motivationList = new List<float>();
    int shakeDuration;

	// Use this for initialization
	void Start () {
        stopWork = false;
        InvokeRepeating("TriggerRandomEvent", 10, 60);
        randomEvents.Add(TrainEvent);
        shakeDuration = 7;
	}
	
	
    void TriggerRandomEvent()
    {

        randomEvents[Random.Range(0, randomEvents.Count)]();
    }


    void TrainEvent()
    {
        motivationList.Clear();
        ScreenShake.shakeDuration = shakeDuration;
        Camera.main.GetComponent<AudioSource>().Play();
        foreach (var npc in DAS.NPC.s_npcList)
        {
            motivationList.Add(npc.myFeelings.Motivation);
            npc.myFeelings.Motivation = 0;
        }

        Invoke("ResetMotivation", shakeDuration);
    }

    void ResetMotivation()
    {
        for(int i = 0; i < DAS.NPC.s_npcList.Count; i++)
        {
            DAS.NPC.s_npcList[i].myFeelings.Motivation = motivationList[i];
        }
    }
}
