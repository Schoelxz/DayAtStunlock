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

    AudioManager audioManager;

	void Start ()
    {
        radiators = FindObjectsOfType<Radiator>();

        motivationLossDuration = Mathf.Clamp(shakeDuration + 5, 0, 25);

        if(DifficultyManager.difficultyScalingEnabled)
        {
            randomEvents.Add(RadiatorEvent);
            InvokeRepeating("TriggerRandomEvent", 2, 60);
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

    private void Update()
    {
        if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium && !randomEvents.Contains(TrainEvent))
        {
            if (DAS.NpcCreator.MaxNumberOfNPCs == DAS.NPC.s_npcList.Count)
            {
                IncreaseDifficulty();
            }
        }

        if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
        {
            IncreaseDifficulty();
        }
        
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

    void IncreaseDifficulty()
    {
        if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium)
        {
            print("Difficulty is now medium");
            CancelInvoke("TriggerRandomEvent");
            randomEvents.Add(TrainEvent);
            InvokeRepeating("TriggerRandomEvent", 2, 45);
        }

        if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
        {
            print("Difficulty is now hard");
            CancelInvoke("TriggerRandomEvent");
            InvokeRepeating("TriggerRandomEvent", 2, 30);
        }
    }

    #region Train 
    void TrainEvent()
    {
        motivationList.Clear();
        ScreenShake.shakeDuration = shakeDuration;
        audioManager.Play("Train");

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
        if(radiators.Length > 0)
        {

            radiators[Random.Range(0, radiators.Length)].RadiatorStart();
        }
    }
    
    #endregion


}
