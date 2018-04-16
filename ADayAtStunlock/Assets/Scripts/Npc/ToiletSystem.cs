using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ToiletSystem : MonoBehaviour
{
    public class Toilet
    {
        public GameObject gameObject;
        public DAS.NPCMovement occupier;
        public List<DAS.NPCMovement> toiletQueue = new List<DAS.NPCMovement>();

        public Toilet(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
        public bool IsOccupied
        {
            get
            {
                if (occupier != null)
                    return true;
                else
                    return false;
            }
        }
    }

    public static ToiletSystem s_myInstance;

    private static List<Toilet> s_toiletPoints = new List<Toilet>();

    public static Dictionary<DAS.NPCMovement, Toilet> s_npcAssignedToToilet = new Dictionary<DAS.NPCMovement, Toilet>();

    private int toiletTrack;

    private void Awake()
    {
        if (s_myInstance == null)
            s_myInstance = this;
        else
            Destroy(this);
    }
    void Start ()
    {
        InitToilets();
	}


    float fdsa;
    private void Update()
    {
        fdsa += Time.deltaTime;
        if (fdsa > 1)
            fdsa = 0;
        else
            return;

        foreach (var toilet in s_toiletPoints)
        {
            if(toilet.toiletQueue.Count != 0)
                Debug.Log(toilet.toiletQueue[0].name);
        }
    }

    #region Functions
    public void InitToilets()
    {
        s_toiletPoints.Clear();
        if (s_toiletPoints.Count == 0)
            foreach (var go in GameObject.FindGameObjectsWithTag("Toilet"))
            {
                s_toiletPoints.Add(new Toilet(go));
            }
    }

    private Toilet NextToilet
    {
        get
        {
            toiletTrack++;
            if (toiletTrack >= s_toiletPoints.Count)
                toiletTrack = 0;
            return s_toiletPoints[toiletTrack];
        }
    }

    /// <summary>
    /// Called in Update.
    /// Random Chance of NPC wanting to go to the toilet.
    /// </summary>
    public void RandomGotoToiletChance(DAS.NPCMovement dasNpcMovement)
    {
        if (Random.Range(0f, 1000f) > 999.7f && dasNpcMovement.IsCurrentlyWorking && dasNpcMovement.workTimeStreak >= 100)
        {
            bool allowedToilet = true;
            for (int i = 0; i < s_toiletPoints.Count; i++)
            {
                if (s_toiletPoints[i].gameObject.transform.position == dasNpcMovement.m_agentRef.destination)
                    allowedToilet = false;
            }
            if (s_npcAssignedToToilet.ContainsKey(dasNpcMovement))
                allowedToilet = false;

            if (allowedToilet)
            {
                dasNpcMovement.isQueued = true;
                if (TryGotoToilet(dasNpcMovement))
                {
                    Debug.Log(dasNpcMovement.name + " goes to toilet");
                }
                else
                {
                    Debug.Log(dasNpcMovement.name + " tried to go to toilet");
                }
            }
        }
    }

    /// <summary>
    /// NPCs destination becomes one of the toilets in the scene.
    /// </summary>
    private bool TryGotoToilet(DAS.NPCMovement dasNpcMovement)
    {
        Toilet temp;
        temp = CheckUnoccupiedToilet();
        if (temp != null && temp.toiletQueue.Count == 0)
        {
            s_npcAssignedToToilet.Add(dasNpcMovement, temp);
            temp.occupier = dasNpcMovement;
            dasNpcMovement.m_agentRef.destination = temp.gameObject.transform.position;
            dasNpcMovement.isAtToilet = true;
            dasNpcMovement.timeInsideDestination = 0;
            dasNpcMovement.m_agentRef.isStopped = false;
            return true;
        }
        else
        {
            if (s_npcAssignedToToilet.ContainsKey(dasNpcMovement))
                Debug.LogWarning("Something went bad");
            s_npcAssignedToToilet.Add(dasNpcMovement, NextToilet);
            AddToQueue(dasNpcMovement);
            dasNpcMovement.timeInsideDestination = 0;
            StartCoroutine(StandInLine(dasNpcMovement));
            dasNpcMovement.m_agentRef.isStopped = false;
            return false;
        }
    }

    public void GotoToilet(DAS.NPCMovement dasNpcMovement)
    {
        s_npcAssignedToToilet[dasNpcMovement].occupier = dasNpcMovement;

        dasNpcMovement.m_agentRef.destination = s_npcAssignedToToilet[dasNpcMovement].gameObject.transform.position;
        dasNpcMovement.isAtToilet = true;
        dasNpcMovement.timeInsideDestination = 0;
        dasNpcMovement.m_agentRef.isStopped = false;
    }

    public bool IsInsideToilet(DAS.NPCMovement dasNpcMovement)
    {
        if (Vector3.Distance(dasNpcMovement.m_agentRef.destination, s_toiletPoints[0].gameObject.transform.position) < 1)
        {
            return true;
        }
        else
            return false;
    }

    public Toilet CheckUnoccupiedToilet()
    {
        foreach (var toilet in s_toiletPoints)
        {
            if (toilet.IsOccupied == false)
            {
                return toilet;
            }
        }
        return null;
    }

    private IEnumerator StandInLine(DAS.NPCMovement dasNpcMovement)
    {
        Debug.Log("Coroutine StandInLine Start");
        while (s_npcAssignedToToilet[dasNpcMovement].toiletQueue[0] != dasNpcMovement)
        {
            yield return new WaitForSeconds(0.2f);

            if(!s_npcAssignedToToilet.ContainsKey(dasNpcMovement))
            {
                Debug.LogWarning("Npc has been unable to leave stand-in-line (or similar) and then been removed from lists");
                break;
            }

            if (s_npcAssignedToToilet[dasNpcMovement].toiletQueue[0] == dasNpcMovement)
            {
                Debug.Log(dasNpcMovement.name + " has now stood in line, proceeding to toilet.");
                break;
            }
            else
            {
                if (s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement) > 0 && s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Count > 0)
                {
                    if(s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement) > 1)
                    {
                        DAS.NPCMovement temp = s_npcAssignedToToilet[dasNpcMovement].toiletQueue[s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement) - 1];
                        if (s_npcAssignedToToilet[dasNpcMovement] == s_toiletPoints[0])
                            dasNpcMovement.m_agentRef.destination = new Vector3((-4.55f) - s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement), 0f, -7.34f);
                        else
                            dasNpcMovement.m_agentRef.destination = new Vector3((4.95f) + s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement), 0f, -7.34f);

                        if (dasNpcMovement.m_agentRef.remainingDistance < 2f)
                            dasNpcMovement.m_agentRef.isStopped = true;
                        else
                            dasNpcMovement.m_agentRef.isStopped = false;
                    }
                    else
                    {
                        if(s_npcAssignedToToilet[dasNpcMovement] == s_toiletPoints[0])
                            dasNpcMovement.m_agentRef.destination = new Vector3(-4.55f, 0f, -7.34f);
                        else
                            dasNpcMovement.m_agentRef.destination = new Vector3(4.95f, 0f, -7.34f);

                        if (dasNpcMovement.m_agentRef.remainingDistance < 2f)
                        {
                            dasNpcMovement.m_agentRef.isStopped = true;
                        }
                        else
                            dasNpcMovement.m_agentRef.isStopped = false;
                    }
                        
                }
            }
        }
        GotoToilet(dasNpcMovement);
        Debug.Log("Coroutine StandInLine End");
    }

    private void AddToQueue(DAS.NPCMovement dasNpcMovement)
    {
        Debug.Log(dasNpcMovement.name + " is being added to queue");
        s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Add(dasNpcMovement);
    }
    public void RemoveFromQueue(DAS.NPCMovement dasNpcMovement)
    {
        Debug.Log(dasNpcMovement.name + " is being removed from queue");
        s_npcAssignedToToilet[dasNpcMovement].occupier = null;
        s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Remove(dasNpcMovement);
        s_npcAssignedToToilet.Remove(dasNpcMovement);
        dasNpcMovement.isQueued = false;
    }
    #endregion
}
