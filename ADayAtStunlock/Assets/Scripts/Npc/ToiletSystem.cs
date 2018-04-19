using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DAS
{
    public class ToiletSystem : MonoBehaviour
    {
        //TODO: Create event breaking the toilets, which queues NPCs for toilet but can never finish.

        public class Toilet
        {
            public GameObject gameObject;
            public DAS.NPCMovement occupier;
            public List<DAS.NPCMovement> toiletQueue = new List<DAS.NPCMovement>();

            public bool broken = false;

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

        #region Variables
        /// <summary>
        /// Singleton class variable.
        /// </summary>
        public static ToiletSystem s_myInstance;

        public GameObject toiletQueue1, toiletQueue2;

        /// <summary>
        /// List of all toilets.
        /// </summary>
        private static List<Toilet> s_toiletPoints = new List<Toilet>();
        /// <summary>
        /// Dictionary of toilets with NPC as key.
        /// </summary>
        private static Dictionary<DAS.NPCMovement, Toilet> s_npcAssignedToToilet = new Dictionary<DAS.NPCMovement, Toilet>();

        /// <summary>
        /// Look at the property NextToilet for more info.
        /// </summary>
        private int toiletTrack;
        #endregion

        #region Start/Awake
        private void Awake()
        {
            if (s_myInstance == null)
                s_myInstance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            InitToilets();
        }
        #endregion

#if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                EveryoneNeedsToiletEvent();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                ToiletBreakEvent();
            }
        }
#endif

        #region Functions
        #region Toilet
        /// <summary>
        /// Find and add all toilets in the scene.
        /// </summary>
        public void InitToilets()
        {
            s_toiletPoints.Clear();
            if (s_toiletPoints.Count == 0)
                foreach (var go in GameObject.FindGameObjectsWithTag("Toilet"))
                {
                    s_toiletPoints.Add(new Toilet(go));
                }
        }

        /// <summary>
        /// Called in Update.
        /// Random Chance of NPC wanting to go to the toilet.
        /// </summary>
        public void RandomGotoToiletChance(DAS.NPCMovement dasNpcMovement)
        {
            if (Random.Range(0f, 1000f) > 999.3f && dasNpcMovement.IsCurrentlyWorking && dasNpcMovement.workTimeStreak >= 100)
            {
                bool allowedToilet = true;

                if (s_npcAssignedToToilet.ContainsKey(dasNpcMovement) || dasNpcMovement.isQueued)
                    allowedToilet = false;

                if (allowedToilet)
                {
                    dasNpcMovement.isQueued = true;
                    if (TryGotoToilet(dasNpcMovement)) { }
                }
            }
        }

        /// <summary>
        /// NPCs destination becomes one of the toilets in the scene.
        /// </summary>
        private bool TryGotoToilet(DAS.NPCMovement dasNpcMovement)
        {
            if (s_npcAssignedToToilet.ContainsKey(dasNpcMovement))
                Debug.LogWarning("Something went bad");

            AddToQueue(dasNpcMovement, NextToilet);
            dasNpcMovement.timeInsideDestination = 0;
            StartCoroutine(StandInLine(dasNpcMovement));
            return false;
        }

        /// <summary>
        /// NPCs destination becomes one of the toilets in the scene.
        /// </summary>
        public void GotoToilet(DAS.NPCMovement dasNpcMovement)
        {
            s_npcAssignedToToilet[dasNpcMovement].occupier = dasNpcMovement;

            dasNpcMovement.m_agentRef.destination = s_npcAssignedToToilet[dasNpcMovement].gameObject.transform.position;
            dasNpcMovement.isAtToilet = true;
            dasNpcMovement.timeInsideDestination = 0;
            dasNpcMovement.m_agentRef.isStopped = false;
        }

        /// <summary>
        /// Checks if NPC is within 0.2 units within its' assigned toilet.
        /// </summary>
        public bool IsInsideToilet(DAS.NPCMovement dasNpcMovement)
        {
            if (Vector3.Distance(dasNpcMovement.m_agentRef.destination, s_npcAssignedToToilet[dasNpcMovement].gameObject.transform.position) < 0.2f)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks all toilets and returns the first available one. If none is available this WILL RETURN NULL!!!
        /// </summary>
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

        /// <summary>
        /// Coroutine function for standing in line to the toilet.
        /// </summary>
        private IEnumerator StandInLine(DAS.NPCMovement dasNpcMovement)
        {
            dasNpcMovement.m_agentRef.isStopped = false;
            Debug.Log("Coroutine StandInLine Start");
            do
            {
                yield return new WaitForSeconds(0.2f);

                while (s_npcAssignedToToilet[dasNpcMovement].toiletQueue[0] != dasNpcMovement)
                {
                    yield return new WaitForSeconds(0.2f);

                    //TODO: find out why the two first in queue. fight over their queue spot until toilets are fixed.

                    //1. if not first in queue and a queue exists.
                    if (s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement) > 0 && s_npcAssignedToToilet[dasNpcMovement].toiletQueue.Count > 0)
                        //3. Which side of the toilets?
                        if (s_npcAssignedToToilet[dasNpcMovement] == s_toiletPoints[0])
                            dasNpcMovement.m_agentRef.destination = toiletQueue1.transform.position +
                                toiletQueue1.transform.forward +
                                (-toiletQueue1.transform.forward * s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement));
                        else
                            dasNpcMovement.m_agentRef.destination = toiletQueue2.transform.position +
                                toiletQueue2.transform.forward +
                                (-toiletQueue2.transform.forward * s_npcAssignedToToilet[dasNpcMovement].toiletQueue.IndexOf(dasNpcMovement));
                    else
                    {
                        if (s_npcAssignedToToilet[dasNpcMovement] == s_toiletPoints[0])
                            dasNpcMovement.m_agentRef.destination = toiletQueue1.transform.position + toiletQueue1.transform.forward;
                        else
                            dasNpcMovement.m_agentRef.destination = toiletQueue2.transform.position + toiletQueue2.transform.forward;
                    }
                }
            } while (s_npcAssignedToToilet[dasNpcMovement].broken);
            GotoToilet(dasNpcMovement);
            Debug.Log("Coroutine StandInLine End");
        }

        private void AddToQueue(DAS.NPCMovement dasNpcMovement, Toilet toiletToQueue)
        {
            Debug.Log(dasNpcMovement.name + " is being added to queue");
            s_npcAssignedToToilet.Add(dasNpcMovement, toiletToQueue);
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

        #region Event
        public void EveryoneNeedsToiletEvent()
        {
            foreach (var npc in NPC.s_npcList)
            {
                if (!s_npcAssignedToToilet.ContainsKey(npc.moveRef))
                    TryGotoToilet(npc.moveRef);
            }
        }

        public void ToiletBreakEvent()
        {
            foreach (var toilet in s_toiletPoints)
            {
                toilet.broken = true;
            }

            Invoke("FixAllToilets", 30);
        }

        public void FixAllToilets()
        {
            foreach (var toilet in s_toiletPoints)
            {
                toilet.broken = false;
            }
        }
        #endregion
        #endregion

        #region Properties
        /// <summary>
        /// Used to even out the toilet uses between all toilets.
        /// </summary>
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
        #endregion
    }
}