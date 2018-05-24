using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour
{
    public static List<System.Action> randomEvents = new List<System.Action>(); //Add random event functions here
    public static List<string> eventHistory = new List<string>();
    public static List<System.Action> s_allEvents = new List<System.Action>(); //All events that exists.

    /// <summary>
    /// Some of the events which gets added to the random event list.
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
        //AudioManager.instance.Play("Train");
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
        for (int i = 0; i < 8; i++)
        {
            Radiator radiator;
            if ((radiator = radiators[UnityEngine.Random.Range(0, radiators.Length)]).isBroken == false)//Checks if the random radiator is broken, if not, it makes it broken and hops out of the loop.
            {
                radiator.RadiatorStart();
                break;
            }
        }
    }
    #endregion

    #region Aliens
    void AlienEvent()
    {
        DAS.NPC npc;

        for (int i = 0; i < alienCount; i++)
        {
            if ((npc = DAS.NPC.s_npcList[UnityEngine.Random.Range(0, DAS.NPC.s_npcList.Count)]).GetComponent<ModelChanger>().isAlien == false && !aliens.Contains(npc))
            {
                aliens.Add(npc);
            }
        }
        spaceshipMovement.updateSpaceship = true;
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

    [SerializeField]
    private Animator m_trainTrack;

    [SerializeField]
    private Animator m_train;

    private List<float> motivationList = new List<float>();
    private int motivationLossDuration;

    private System.Action lastEvent;

    //Radiator stuff
    private Radiator[] radiators;

    //Spaceship stuff
    private int alienCount;
    public List<DAS.NPC> aliens = new List<DAS.NPC>();
    private SpaceshipMovement spaceshipMovement;

    private int eventDelayEasy, eventDelayMedium, eventDelayHard;

    //Amount of events that have been called.
    private int eventCounter = 0;

    void Start ()
    {
        //Clear the list of events on Start (to avoid filling the list on restarts)
        randomEvents.Clear();
        s_allEvents.Clear();
        eventHistory.Clear();

        //As name implies
        AddAllEventsToAllEventsList();

        //Train Stuff
        m_trainTrack.gameObject.SetActive(false);
        motivationLossDuration = Mathf.Clamp(shakeDuration + 3, 0, 25);

        //Spaceship stuff
        spaceshipMovement = GameObject.FindObjectOfType<SpaceshipMovement>();
        alienCount = 8;

        //Radiator stuff
        radiators = FindObjectsOfType<Radiator>();

        eventDelayEasy = 50;
        eventDelayMedium = 30;
        eventDelayHard = 15;

        //Event order
        randomEvents.Add(RadiatorEvent);
        randomEvents.Add(ToiletBreaksEvent);
        randomEvents.Add(AlienEvent);
        randomEvents.Add(RadiatorEvent);
        randomEvents.Add(TrainEvent);
        randomEvents.Add(BollHav.MyInstance.StartBollHav);
        randomEvents.Add(RadiatorEvent);
        randomEvents.Add(BollHav.MyInstance.StartBollHav);
        randomEvents.Add(ToiletBreaksEvent);
        randomEvents.Add(TrainEvent);
        randomEvents.Add(BollHav.MyInstance.StartBollHav);

        //Start events
        StartCoroutine(TriggerRandomEvent(eventDelayEasy, eventDelayEasy));
        
        Debug.Assert(AudioManager.instance, "No AudioManager.instance exists!!!");
    }

    private void AddAllEventsToAllEventsList()
    {
        s_allEvents.Add(AlienEvent);
        s_allEvents.Add(TrainEvent);
        s_allEvents.Add(RadiatorEvent);
        s_allEvents.Add(ToiletBreaksEvent);
        s_allEvents.Add(DAS.ToiletSystem.s_myInstance.EveryoneNeedsToiletEvent);
        s_allEvents.Add(DAS.NpcCreator.ToggleGUICheat);
        s_allEvents.Add(BollHav.MyInstance.StartBollHav);
    }

    private IEnumerator TriggerRandomEvent(float startWaitTime, float loopWaitTime)
    {
        yield return new WaitForSeconds(startWaitTime);

        while (true)
        {
            lastEvent = EventDisplay.FunctionTriggered(randomEvents[eventCounter++ % randomEvents.Count]);

            eventHistory.Add(lastEvent.Method.Name + " " + eventCounter);

            yield return new WaitForSeconds(loopWaitTime);
        }
    }

    public void WhenDifficultyIncreases()
    {
        //Medium Difficulty
        if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium)
        {
            StopAllCoroutines();
            StartCoroutine(TriggerRandomEvent(20, eventDelayMedium));
        }

        //Hard difficulty
        if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
        {
            StopAllCoroutines();
            StartCoroutine(TriggerRandomEvent(15, eventDelayHard));
        }
    }
}
