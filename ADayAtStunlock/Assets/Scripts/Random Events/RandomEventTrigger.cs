using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventTrigger : MonoBehaviour
{
    // Some of the events which gets added to the random event list.
    #region Events:
    #region Train
    private bool hasMotivationReset = true;
    void TrainEvent()
    {
        if (!hasMotivationReset)
            return;
        m_motivationList.Clear();
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
            m_motivationList.Add(npc.myFeelings.Motivation);
            npc.myFeelings.Motivation = 0;
        }

        Invoke("ResetMotivation", m_motivationLossDuration);
    }

    void ResetMotivation()
    {
        for (int i = 0; i < m_motivationList.Count - 1; i++)
        {
            DAS.NPC.s_npcList[i].myFeelings.Motivation += m_motivationList[i];
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

        for (int i = 0; i < m_alienCount; i++)
        {
            if ((npc = DAS.NPC.s_npcList[UnityEngine.Random.Range(0, DAS.NPC.s_npcList.Count)]).GetComponent<ModelChanger>().isAlien == false && !aliens.Contains(npc))
            {
                aliens.Add(npc);
            }
        }
        m_spaceshipMovement.updateSpaceship = true;
    }
    #endregion

    #region Toilet
    void ToiletBreaksEvent()
    {
        DAS.ToiletSystem.s_myInstance.ToiletBreakEvent();
    }

    #endregion
    #endregion

    #region Variables
    /// <summary>
    /// List of events that are played in order of the list.
    /// </summary>
    private static List<System.Action> s_orderedEventList = new List<System.Action>();
    /// <summary>
    /// List of names of events that has been played, ordered from first played event to last played event.
    /// </summary>
    public static List<string> s_eventHistory = new List<string>();
    /// <summary>
    /// All events that exists (used for cheats).
    /// </summary>
    public static List<System.Action> s_allEvents = new List<System.Action>(); 
    /// <summary>
    /// Most recent event that was played.
    /// </summary>
    private System.Action m_lastEvent;

    [Range(1, 20)]
    [Tooltip("Determines for how long it will shake when event is triggered. Also determines how long NPCs motivation is lost (shake duration + 5 = motivation loss duration)!")]
    public int shakeDuration = 6;
    [SerializeField]
    private Animator m_trainTrack;
    [SerializeField]
    private Animator m_train;
    private List<float> m_motivationList = new List<float>();
    private int m_motivationLossDuration;

    private Radiator[] radiators;

    public List<DAS.NPC> aliens = new List<DAS.NPC>();
    private int m_alienCount;
    private SpaceshipMovement m_spaceshipMovement;

    private int m_eventDelayEasy, m_eventDelayMedium, m_eventDelayHard;

    //Amount of events that have been called.
    private int m_eventCounter = 0;
    #endregion

    void Start ()
    {
        //Clear the list of events on Start (to avoid filling the list on restarts)
        s_orderedEventList.Clear();
        s_allEvents.Clear();
        s_eventHistory.Clear();

        //Train Stuff
        m_trainTrack.gameObject.SetActive(false);
        m_motivationLossDuration = Mathf.Clamp(shakeDuration + 3, 0, 25);

        //Spaceship stuff
        m_spaceshipMovement = GameObject.FindObjectOfType<SpaceshipMovement>();
        m_alienCount = 8;

        //Radiator stuff
        radiators = FindObjectsOfType<Radiator>();

        m_eventDelayEasy = 50;
        m_eventDelayMedium = 30;
        m_eventDelayHard = 15;

        //Event order
        AddEventOrder();

        //Start events
        StartCoroutine(TriggerEventsInOrder(m_eventDelayEasy, m_eventDelayEasy));

        AddAllEventsToAllEventsList();

        Debug.Assert(AudioManager.instance, "No AudioManager.instance exists!!!");
    }

    private void AddEventOrder()
    { 
        s_orderedEventList.Add(RadiatorEvent);
        s_orderedEventList.Add(ToiletBreaksEvent);
        s_orderedEventList.Add(AlienEvent);
        s_orderedEventList.Add(RadiatorEvent);
        s_orderedEventList.Add(TrainEvent);
        s_orderedEventList.Add(BollHav.MyInstance.StartBollHav);
        s_orderedEventList.Add(RadiatorEvent);
        s_orderedEventList.Add(BollHav.MyInstance.StartBollHav);
        s_orderedEventList.Add(ToiletBreaksEvent);
        s_orderedEventList.Add(TrainEvent);
        s_orderedEventList.Add(BollHav.MyInstance.StartBollHav);
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

    private IEnumerator TriggerEventsInOrder(float startWaitTime, float loopWaitTime)
    {
        yield return new WaitForSeconds(startWaitTime);

        while (true)
        {
            m_lastEvent = EventDisplay.FunctionTriggered(s_orderedEventList[m_eventCounter++ % s_orderedEventList.Count]);

            s_eventHistory.Add(m_lastEvent.Method.Name + " " + m_eventCounter);

            yield return new WaitForSeconds(loopWaitTime);
        }
    }

    /// <summary>
    /// Called from DifficultyManager when difficulty is bumped up.
    /// </summary>
    public void WhenDifficultyIncreases()
    {
        BollHav.MyInstance.StartBollHav();

        //Medium Difficulty
        if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium)
        {
            StopAllCoroutines();
            StartCoroutine(TriggerEventsInOrder(20, m_eventDelayMedium));
        }

        //Hard difficulty
        if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
        {
            StopAllCoroutines();
            StartCoroutine(TriggerEventsInOrder(15, m_eventDelayHard));
        }
    }
}
