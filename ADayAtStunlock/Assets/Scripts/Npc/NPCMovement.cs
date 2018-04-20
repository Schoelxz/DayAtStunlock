using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DAS
{
    public class NPCMovement : MonoBehaviour
    {
        #region Structs
        private static class Toilet
        {
            public static List<GameObject> s_toiletPoints = new List<GameObject>();
            private static int toiletTrack;

            public static Vector3 GetAToiletPosition
            {
                get
                {
                    toiletTrack++;
                    if (toiletTrack == s_toiletPoints.Count)
                        toiletTrack = 0;
                    return s_toiletPoints[toiletTrack].transform.position;
                }
            }

            public static void InitToilets()
            {
                s_toiletPoints.Clear();
                if (s_toiletPoints.Count == 0)
                    s_toiletPoints.AddRange(GameObject.FindGameObjectsWithTag("Toilet"));
            }
        }
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
        
        private Transform myWorkSeat;
        private NavMeshAgent agentRef;
        private float timeInsideDestination;
        private float workTimeStreak;
        private AgentValues agentValues;
        private Animator[] m_animator;
        private Animator currentAnimator;

        private DAS.NPC myNpcRef;

       // private bool gameHasBeenPaused = false;

        // Delta time
        private float dt;

        private void Awake()
        {
            WorkSeatTemp.InitWorkSeats();
            Toilet.InitToilets();

            // All getcomponent functions are called inside this function, returning false if it fails.
            if (!SetAllGetComponents())
                Debug.LogAssertion("GetComponent Failed inside SetAllGetComponents function.");
        }

        void Start()
        {
            myNpcRef = gameObject.GetComponent<NPC>();

            //Find Animator
            m_animator = GetComponentsInChildren<Animator>(true);
            Debug.Assert(agentRef);

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
            myWorkSeat = myNpcRef.myWorkSeat.workSeatGameObject.transform;

            // Assert
            Debug.Assert(agentRef);
            Debug.Assert(myWorkSeat);

            // NavMeshAgent starts disabled because Unity has a bug involving it and giving the wrong position.
            agentRef.enabled = true;

            InvokeRepeating("RandomlySetAvoidancePriority", 1, 2);

            // Our NPC starts by going to its work seat.
            agentRef.destination = myWorkSeat.position;
        }

        void Update()
        {
            //Animate work
           if(IsDestinationMyWorkSeat && IsCurrentlyWorking && GetComponent<NPC>().myFeelings.Motivation != 0)
            {
                currentAnimator.SetBool("Pickup 0", true);
            }
           else
                currentAnimator.SetBool("Pickup 0", false);
            #region old pause
            /*
            if(gameHasBeenPaused == false)
                agentValues.velocity = agentRef.velocity;

            if (gameHasBeenPaused && !DAS.TimeSystem.IsGamePaused)
            {
                gameHasBeenPaused = false;
                agentRef.velocity = agentValues.velocity;
            }

            if(DAS.TimeSystem.IsGamePaused)
            {
                gameHasBeenPaused = true;
                agentRef.isStopped = true;
                agentRef.velocity = Vector3.zero;
            }
            else*/
            // {
            #endregion

            // Rotate towards our desk if we are basically on our chair in our work seat and working.
            if (IsCurrentlyWorking && Vector3.Distance(agentRef.destination, transform.position) < 0.1f)
                RotateTowardsDesk();
           else // else we should not be stopped in our movement
                agentRef.isStopped = false;

            #region dt call reducer
            //+++ Reduce update calls to 10 times per second.
            dt += Time.deltaTime;
            if (dt >= 0.1f && !DAS.TimeSystem.IsGamePaused)
            { dt = 0; }
            else
                return;
            //---
            #endregion

            // Random Chance of NPC wanting to go to the toilet.
            RandomGotoToiletChance();

            // Check time an NPC has been inside of its destination (not counting work destination).
            if (agentRef.remainingDistance <= 1f && !IsDestinationMyWorkSeat)
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
            if (timeInsideDestination >= 50)
            {
                agentRef.destination = myWorkSeat.position;
                timeInsideDestination = 0;
                agentRef.isStopped = false;
            }

            //Movement Animation
            currentAnimator.SetFloat("MoveSpeed", agentRef.velocity.magnitude);
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
            if (!(agentRef = GetComponent<NavMeshAgent>()))
                return false;

            return true;
        }

        /// <summary>
        /// NPCs destination becomes one of the toilets in the scene.
        /// </summary>
        private void GotoToilet()
        {
            timeInsideDestination = 0;
            agentRef.destination = Toilet.GetAToiletPosition;
            agentRef.isStopped = false;
        }

        /// <summary>
        /// Called in Update.
        /// Random Chance of NPC wanting to go to the toilet.
        /// </summary>
        private void RandomGotoToiletChance()
        {
            if (Random.Range(0f, 1000f) > 999.7f && IsCurrentlyWorking && workTimeStreak >= 100)
            {
                bool allowedToilet = true;
                for (int i = 0; i < Toilet.s_toiletPoints.Count; i++)
                {
                    if (Toilet.s_toiletPoints[i].transform.position == agentRef.destination)
                        allowedToilet = false;
                }
                if (allowedToilet)
                    GotoToilet();
            }
        }

        /// <summary>
        /// Returns true if NPC destination is its' work seat.
        /// </summary>
        private bool IsDestinationMyWorkSeat
        {
            get
            {
                // Code has weirdly prompted NullReferenceExceptions here, therefore if agentref == null was added.
                if (agentRef == null)
                {
                    Debug.Assert(agentRef);
                    return false;
                }

                if (agentRef.destination.x == myWorkSeat.position.x && agentRef.destination.z == myWorkSeat.position.z)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Returns true if #1. Destination is workseat. #2. Is within (1.5) units of its' workseat.
        /// </summary>
        public bool IsCurrentlyWorking
        {
            get
            {
                if (IsDestinationMyWorkSeat)
                {
                    if (Vector3.Distance(transform.position, myWorkSeat.position) < 0.5f)
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
            agentRef.avoidancePriority = Random.Range(1, 100);
        }

        private void RotateTowardsDesk()
        {
            // Stop our agent from fidgeting in his seat.
            agentRef.isStopped = true;
            // targetDir is our work seats position + forward its own direction (roughly our desk's position)
            Vector3 targetDir = (myWorkSeat.position + (-myWorkSeat.forward) - myWorkSeat.position);
            // How fast we turn every frame.
            float step = 5 * Time.deltaTime;
            // Our complete rotate towards direction.
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);

            // Draw ray towards the position we're rotating towards.
            //Debug.DrawRay(transform.position, newDir, Color.red, 3);

            // Apply the rotate towards.
            transform.rotation = Quaternion.LookRotation(newDir);
        }

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
        #endregion
    }
}