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

    private static Dictionary<DAS.NPCMovement, Toilet> s_npcAssignedToToilet = new Dictionary<DAS.NPCMovement, Toilet>();

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
            if (toiletTrack > s_toiletPoints.Count)
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
            if (allowedToilet)
                if (GotoToilet(dasNpcMovement))
                { }
        }
    }

    /// <summary>
    /// NPCs destination becomes one of the toilets in the scene.
    /// </summary>
    private bool GotoToilet(DAS.NPCMovement dasNpcMovement)
    {
        Toilet temp;
        temp = CheckUnoccupiedToilet();
        if (temp != null)
        {
            temp.occupier = dasNpcMovement;
            dasNpcMovement.m_agentRef.destination = temp.gameObject.transform.position;
            dasNpcMovement.timeInsideDestination = 0;
            dasNpcMovement.m_agentRef.isStopped = false;
            return true;
        }
        else
        {
            s_npcAssignedToToilet.Add(dasNpcMovement, NextToilet);
            AddToQueue(dasNpcMovement);
            dasNpcMovement.timeInsideDestination = 0;
            dasNpcMovement.m_agentRef.isStopped = false;
            return false;
        }
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

    private Toilet CheckUnoccupiedToilet()
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
        while (CheckUnoccupiedToilet().toiletQueue[0] != dasNpcMovement)
        {
            yield return new WaitForSeconds(0.2f);

            if (s_npcAssignedToToilet[dasNpcMovement].toiletQueue[0] == dasNpcMovement)
            {
                s_npcAssignedToToilet[dasNpcMovement].occupier = dasNpcMovement;
                GotoToilet(dasNpcMovement);
            }
        }
    }

    private void AddToQueue(DAS.NPCMovement dasNpcMovement)
    {
        s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Add(dasNpcMovement);
    }
    public void RemoveFromQueue(DAS.NPCMovement dasNpcMovement)
    {
        s_npcAssignedToToilet[dasNpcMovement].occupier = null;
        s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Remove(dasNpcMovement);
        s_npcAssignedToToilet.Remove(dasNpcMovement);
    }
    #endregion
}
