using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DAS
{
    public class NPCMovement : MonoBehaviour
    {
        #region Structs
        private static class WorkSeatTemp
        {
            public static List<GameObject> s_allWorkSeats = new List<GameObject>();

            public static int GetWorkSeatAmount
            {
                get
                {
                    return s_allWorkSeats.Count;
                }
            }

            public static void InitWorkSeats()
            {
                s_allWorkSeats.Clear();
                if (s_allWorkSeats.Count == 0)
                    s_allWorkSeats.AddRange(GameObject.FindGameObjectsWithTag("WorkSeat"));
            }
        }
        private struct AgentValues
        {
            public Vector3 velocity;
        }
        #endregion

        private static List<NPCMovement> s_allNPCs = new List<NPCMovement>();

        private Transform m_myWorkSeat;
        public NavMeshAgent m_agentRef;
        private AgentValues m_agentValues;
        private Animator[] m_animator;
        private Animator currentAnimator;
        public DAS.NPC m_myNpcRef;

        public float timeInsideDestination;
        public float workTimeStreak;

        public bool isAtToilet = false;
        public bool isQueued = false;

        AudioManager audioManager;

        // Delta time
        private float dt;

        private void Awake()
        {
            WorkSeatTemp.InitWorkSeats();
        }

        void Start()
        {
            //Find AudioManager
            audioManager = FindObjectOfType<AudioManager>();
            // All getcomponent functions are called inside this function, returning false if it fails.
            if (!SetAllGetComponents())
                Debug.LogAssertion("GetComponent Failed inside SetAllGetComponents function.");

            //Find Animator
            m_animator = GetComponentsInChildren<Animator>(true);
            Debug.Assert(m_agentRef);

            for (int i = 0; i < m_animator.Length; i++)
            {
                if(m_animator[i].gameObject.activeInHierarchy)
                {
                    currentAnimator = m_animator[i];
                }
            }
            // Add this NPC to the static list of NPCs.
            s_allNPCs.Add(this);

            // Assign this NPCs' work seat.
            m_myWorkSeat = m_myNpcRef.myWorkSeat.workSeatGameObject.transform;

            // Assert
            Debug.Assert(m_myWorkSeat);
            Debug.Assert(m_agentRef);
            Debug.Assert(currentAnimator);
            Debug.Assert(m_myNpcRef);

            // NavMeshAgent starts disabled because Unity has a bug involving it and giving the wrong position.
            m_agentRef.enabled = true;

            // To give NPCs the chance to walk past eachother. Set a random number between 1-100 for the avoidance priority on the navAgent every 2nd second.
            InvokeRepeating("RandomlySetAvoidancePriority", 1, 2);

            // Our NPC starts by going to its work seat.
            m_agentRef.destination = m_myWorkSeat.position;
        }

        void Update()
        {
            //Animate work
            if (IsCurrentlyWorking && m_myNpcRef.myFeelings.Motivation != 0)
            {
                //Animate work on
                currentAnimator.SetBool("Pickup 0", true);
                //Work sound on
                if(!audioManager.isPlaying("NPCWorking", gameObject))
                {
                    audioManager.PlaySound("NPCWorking", gameObject);
                }
            }
            else
            {
                //Animate work off
                currentAnimator.SetBool("Pickup 0", false);
                //Work sound off
                if (audioManager.isPlaying("NPCWorking", gameObject))
                {
                    audioManager.StopSound("NPCWorking", gameObject);
                }
            }


            // Rotate towards our desk if we are basically on our chair in our work seat and working.
            if (IsCurrentlyWorking && Vector3.Distance(m_agentRef.destination, transform.position) < 0.1f)
                RotateTowardsDesk();
            // else we should not be stopped in our movement
            else
                m_agentRef.isStopped = false;

            #region dt call reducer
            //+++ Reduce update calls to 10 times per in-game second.
            dt += TimeSystem.DeltaTime;
            if (dt >= 0.1f && !DAS.TimeSystem.IsGamePaused)
            { dt = 0; }
            else
                return;
            //---
            #endregion

            // Random Chance of NPC wanting to go to the toilet. Can only happen if one is currently working and is not currently queued for
            if (IsCurrentlyWorking && !isQueued)
                ToiletSystem.s_myInstance.RandomGotoToiletChance(this);

            // Check time an NPC has been inside of its destination (not counting work destination).
            if (m_agentRef.remainingDistance <= 1f && !IsDestinationMyWorkSeat && isAtToilet)
            {
                timeInsideDestination++;
                GetComponent<NPC>().myFeelings.Happiness += 0.01f;
            }

            // Count how long we have been working since interupption.
            if (IsCurrentlyWorking)
                workTimeStreak++;
            else
                workTimeStreak = 0;

            // If our NPC has been at a destination for longer than (5) seconds, go back to work.
            CheckAwayTimeTriggerReturn(25);

            //Movement Animation
            currentAnimator.SetFloat("MoveSpeed", m_agentRef.velocity.magnitude);
        }

        private void OnDestroy()
        {
            s_allNPCs.Remove(this);
        }

        #region Functions
        /// <summary>
        /// Initializes variables calling upon GetComponent
        /// <para>Returns false if GetComponent fails on any variable.</para>
        /// </summary>
        /// <returns></returns>
        private bool SetAllGetComponents()
        {
            if (!(m_agentRef = GetComponent<NavMeshAgent>()))
                return false;

            if (!(m_myNpcRef = gameObject.GetComponent<NPC>()))
                return false;

            if (!(currentAnimator = GetComponentInChildren<Animator>()))
                return false;

            return true;
        }

        /// <summary>
        /// Returns true if NPC destination is its' work seat.
        /// </summary>
        private bool IsDestinationMyWorkSeat
        {
            get
            {
                // Code has weirdly prompted NullReferenceExceptions here, therefore if agentref == null was added.
                if (m_agentRef == null)
                {
                    Debug.Assert(m_agentRef);
                    return false;
                }

                if (m_agentRef.destination.x == m_myWorkSeat.position.x && m_agentRef.destination.z == m_myWorkSeat.position.z)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if #1. Destination is workseat. #2. Is within (0.5) units of its' workseat.
        /// </summary>
        public bool IsCurrentlyWorking
        {
            get
            {
                if (IsDestinationMyWorkSeat)
                {
                    if (Vector3.Distance(transform.position, m_myWorkSeat.position) < 0.5f)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// By randomly setting the avoidancepriorities,
        /// NPCs will more easily choose who to avoid and who to not,
        /// making a more "fluid" movemnt of not stubbornly walking into eachother.
        /// </summary>
        private void RandomlySetAvoidancePriority()
        {
            m_agentRef.avoidancePriority = Random.Range(1, 100);
        }

        /// <summary>
        /// Rotates our NPC so he looks towards his desk.
        /// </summary>
        private void RotateTowardsDesk()
        {
            // Stop our agent from fidgeting in his seat.
            m_agentRef.isStopped = true;
            // targetDir is our work seats position + forward its own direction (roughly our desk's position)
            Vector3 targetDir = (m_myWorkSeat.position + (-m_myWorkSeat.forward) - m_myWorkSeat.position);
            // How fast we turn every frame.
            float step = 5 * Time.deltaTime;
            // Our complete rotate towards direction.
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
            // Apply the rotate towards.
            transform.rotation = Quaternion.LookRotation(newDir);
        }

        //Changes the animator based on whether the "Alien" or the "NPC" model is active in the scene. This function is executed from the ModelChanger script.
        public void ToggleAnimator()
        {
            for (int i = 0; i < m_animator.Length; i++)
            {
                if(m_animator[i].gameObject.activeInHierarchy)
                {
                    currentAnimator = m_animator[i];
                }
            }
        }

        /// <summary>
        /// Called in update+ Checks if NPC has been away at its' destination for a set amount of time. If it has, trigger the NPC to come back to work.
        /// </summary>
        private void CheckAwayTimeTriggerReturn(int timeAllowedAway)
        {
            if (timeInsideDestination >= timeAllowedAway)
            {
                ToiletSystem.s_myInstance.RemoveFromQueue(this);
                m_agentRef.destination = m_myWorkSeat.position;
                isAtToilet = false;
                timeInsideDestination = 0;
                m_agentRef.isStopped = false;
            }
        }

        #endregion
    }
}
