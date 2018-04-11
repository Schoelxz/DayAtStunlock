using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour
{
    public List<System.Action> randomEvents = new List<System.Action>(); //Add random event functions here

    [Range(1, 20)]
    [Tooltip("Determines for how long it will shake when event is triggered. Also determines how long NPCs motivation is lost (shake duration + 5 = motivation loss duration)!")]
    public int shakeDuration = 6;

    private List<float> motivationList = new List<float>();
    private Radiator[] radiators;
    private int motivationLossDuration;
    AudioManager audioManager;

    int eventDelayEasy;
    int eventDelayMedium;
    int eventDelayHard;

    void Start ()
    {
        radiators = FindObjectsOfType<Radiator>();

        motivationLossDuration = Mathf.Clamp(shakeDuration + 3, 0, 25);

        eventDelayEasy = 50;
        eventDelayMedium = 40;
        eventDelayHard = 30;

        if(DifficultyManager.difficultyScalingEnabled)
        {
            randomEvents.Add(RadiatorEvent);
            InvokeRepeating("TriggerRandomEvent", 2, eventDelayEasy);
        }
        else
        {
            randomEvents.Add(TrainEvent);
            randomEvents.Add(RadiatorEvent);
            StartCoroutine(StartInvokeRepeatingWhen());
        }

        audioManager = FindObjectOfType<AudioManager>();
        //Debug.Assert(GetComponent<AudioSource>(), gameObject.name + " has no audio source. Script RandomEventTrigger requires it!");
    }
    
    //Makes sure to start the random events after all npcs have spawned
    IEnumerator StartInvokeRepeatingWhen()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            // Starts InvokeRepeating when All Npcs has been created.
            if(DAS.NpcCreator.MaxNumberOfNPCsByWorkseatAmount == DAS.NPC.s_npcList.Count)
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

    public void IncreaseDifficulty()
    {
        //Medium Difficulty
        if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium)
        {
            CancelInvoke("TriggerRandomEvent");
            randomEvents.Add(TrainEvent);
            InvokeRepeating("TriggerRandomEvent", 2, eventDelayMedium);

        }
        
        //Hard difficulty
        if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
        {
            CancelInvoke("TriggerRandomEvent");
            //Add events to randomevents list here if we have any new ones. 
            InvokeRepeating("TriggerRandomEvent", 2, eventDelayHard);
        }
    }

    #region Train 
    private bool hasMotivationReset = true;
    void TrainEvent()
    {
        if (!hasMotivationReset)
            return;
        motivationList.Clear();
        ScreenShake.shakeDuration = shakeDuration;
        //audioManager.Play("Train");
        hasMotivationReset = false;
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
        hasMotivationReset = true;
    }
    #endregion
    
    #region Radiator

    void RadiatorEvent()
    {
        if(radiators.Length > 0)
        {
            radiators[Random.Range(0, radiators.Length)].RadiatorStart();
        }
    }
    
    #endregion


}
