using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour
{
    public List<System.Action> randomEvents = new List<System.Action>(); //Add random event functions here

    [Range(1, 20)]
    [Tooltip("Determines for how long it will shake when event is triggered. Also determines how long NPCs motivation is lost (shake duration + 5 = motivation loss duration)!")]
    public int shakeDuration = 10;

    private List<float> motivationList = new List<float>();

    private Radiator[] radiators;

    private int motivationLossDuration;

	void Start ()
    {
        radiators = FindObjectsOfType<Radiator>();

        motivationLossDuration = Mathf.Clamp(shakeDuration + 5, 0, 25);

        //randomEvents.Add(TrainEvent);

        randomEvents.Add(RadiatorEvent);

        StartCoroutine(StartInvokeRepeatingWhen());
        
        Debug.Assert(GetComponent<AudioSource>(), gameObject.name + " has no audio source. Script RandomEventTrigger requires it!");
	}

    //Makes sure to start the random events after all npcs have spawned
    IEnumerator StartInvokeRepeatingWhen()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            // Starts InvokeRepeating when All Npcs has been created.
            if(DAS.NpcCreator.MaxNumberOfNPCs == DAS.NPC.s_npcList.Count)
            {
                InvokeRepeating("TriggerRandomEvent", 2, 60);
                break;
            }
        }
        Debug.Log("coroutine end");
    }

    void TriggerRandomEvent()
    {
        randomEvents[Random.Range(0, randomEvents.Count)]();
    }

    #region Train 
    void TrainEvent()
    {
        motivationList.Clear();
        ScreenShake.shakeDuration = shakeDuration;
        GetComponent<AudioSource>().Play();
        foreach (var npc in DAS.NPC.s_npcList)
        {
            motivationList.Add(npc.myFeelings.Motivation);
            npc.myFeelings.Motivation = 0;
        }

        Invoke("ResetMotivation", motivationLossDuration);
    }

    void ResetMotivation()
    {
        for(int i = 0; i < motivationList.Count - 1; i++)
        {
            DAS.NPC.s_npcList[i].myFeelings.Motivation += motivationList[i];
        }
    }
    #endregion


    #region Radiator

    void RadiatorEvent()
    {
            radiators[Random.Range(0, radiators.Length)].RadiatorNoise();
        
    }
    
    #endregion
}
