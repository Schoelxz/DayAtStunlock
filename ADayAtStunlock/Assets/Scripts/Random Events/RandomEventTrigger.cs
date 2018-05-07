﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour
{
    public static List<System.Action> randomEvents = new List<System.Action>(); //Add random event functions here

    /// <summary>
    /// All the events which gets added to the random event list.
    /// </summary>
    #region Events:
    #region Train
    private bool hasMotivationReset = true;
    void TrainEvent()
    {
        if (!hasMotivationReset)
            return;
        motivationList.Clear();
        ScreenShake.shakeDuration = shakeDuration;

        //Turn on trains
        m_trainTrack.gameObject.SetActive(true);
        m_trainTrack.SetBool("IsActive", true);
        m_train.SetBool("IsMoving", true);
        //Train sound
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
        for (int i = 0; i < motivationList.Count - 1; i++)
        {
            DAS.NPC.s_npcList[i].myFeelings.Motivation += motivationList[i];
        }
        hasMotivationReset = true;
        //Turn off trains
        m_trainTrack.SetBool("IsActive", false);
        m_train.SetBool("IsMoving", false);

        Invoke("InvokeOnlyFunctionHideTrainTrack", 1);
    }

    private void InvokeOnlyFunctionHideTrainTrack()
    {
        m_trainTrack.gameObject.SetActive(false);
    }
    #endregion

    #region Radiator

    void RadiatorEvent()
    {
        bool foundBroken = false;
        for (int i = 0; i < 8; i++)
        {
            Radiator radiator;
            if ((radiator = radiators[Random.Range(0, radiators.Length)]).isBroken == false)//Checks if the random radiator is broken, if not, it makes it broken and hops out of the loop.
            {
                radiator.RadiatorStart();
                foundBroken = true;
                break;
            }
        }

        if (foundBroken == false)
        {
            TriggerRandomEvent();
        }
    }

    #endregion

    #region Aliens

    void AlienEvent()
    {
        DAS.NPC npc;

        for (int i = 0; i < alienCount; i++)
        {
            if ((npc = DAS.NPC.s_npcList[Random.Range(0, DAS.NPC.s_npcList.Count)]).GetComponent<ModelChanger>().isAlien == false && !aliens.Contains(npc))
            {
                
                aliens.Add(npc);
            }
        }
        
    }
    

    #endregion

    #region Toilet
    void ToiletBreaksEvent()
    {
        DAS.ToiletSystem.s_myInstance.ToiletBreakEvent();
    }

    #endregion
    #endregion

    //Train Stuff
    [Range(1, 20)]
    [Tooltip("Determines for how long it will shake when event is triggered. Also determines how long NPCs motivation is lost (shake duration + 5 = motivation loss duration)!")]
    public int shakeDuration = 6;
    //TrainPrefab
    [SerializeField]
    //[Tooltip("Put TrainEventPrefab here")]
    Animator m_trainTrack;
    [SerializeField]
    Animator m_train;
    private List<float> motivationList = new List<float>();
    private int motivationLossDuration;

    //Radiator stuff
    private Radiator[] radiators;

    //Spaceship stuff
    private GameObject spaceship;
    int alienCount;

    AudioManager audioManager;
    int eventDelayEasy;
    int eventDelayMedium;
    int eventDelayHard;

    public List<DAS.NPC> aliens = new List<DAS.NPC>();

    void Start ()
    {
        //Clear the list of events on Start (to avoid filling the list on restarts)
        randomEvents.Clear();

        //Train Stuff
        m_trainTrack.gameObject.SetActive(false);
        motivationLossDuration = Mathf.Clamp(shakeDuration + 3, 0, 25);

        //Spaceship stuff
        spaceship = GameObject.FindGameObjectWithTag("Spaceship");
        //spaceship.SetActive(false);
        alienCount = 8;

        //Radiator stuff
        radiators = FindObjectsOfType<Radiator>();

        eventDelayEasy = 50;
        eventDelayMedium = 40;
        eventDelayHard = 20;

        if(DifficultyManager.difficultyScalingEnabled)
        {
            randomEvents.Add(RadiatorEvent);
            randomEvents.Add(AlienEvent);
            randomEvents.Add(ToiletBreaksEvent);
            InvokeRepeating("TriggerRandomEvent", 20, eventDelayEasy);
        }
        else
        {
            randomEvents.Add(AlienEvent);
            randomEvents.Add(TrainEvent);
            randomEvents.Add(RadiatorEvent);
            randomEvents.Add(ToiletBreaksEvent);
            StartCoroutine(StartInvokeRepeatingWhen());
        }

        audioManager = FindObjectOfType<AudioManager>();
        Debug.Assert(audioManager, "No audiomanager exists!!!");
    }

    void TriggerRandomEvent()
    {
        EventDisplay.FunctionTriggered(randomEvents[Random.Range(0, randomEvents.Count)]);
    }

    public void WhenDifficultyIncreases()
    {
        //Medium Difficulty
        if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium)
        {
            CancelInvoke("TriggerRandomEvent");
            randomEvents.Add(TrainEvent);
            InvokeRepeating("TriggerRandomEvent", 20, eventDelayMedium);
        }

        //Hard difficulty
        if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
        {
            CancelInvoke("TriggerRandomEvent");
            //Add events to randomevents list here if we have any new ones.
            randomEvents.Add(TrainEvent);
            randomEvents.Add(ToiletBreaksEvent);
            randomEvents.Add(RadiatorEvent);
            InvokeRepeating("TriggerRandomEvent", 20, eventDelayHard);
        }
    }

    //Makes sure to start the random events after all npcs have spawned
    IEnumerator StartInvokeRepeatingWhen()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            // Starts InvokeRepeating when All Npcs has been created.
            if (DAS.NpcCreator.MaxNumberOfNPCsByWorkseatAmount == DAS.NPC.s_npcList.Count)
            {
                InvokeRepeating("TriggerRandomEvent", 2, 60);
                break;
            }
        }
    }
}
