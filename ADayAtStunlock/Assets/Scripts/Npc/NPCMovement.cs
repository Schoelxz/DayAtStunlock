using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DAS
{
    public class NPCMovement : MonoBehaviour
    {
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
                if (s_toiletPoints.Count == 0)
                    s_toiletPoints.AddRange(GameObject.FindGameObjectsWithTag("Toilet"));
            }
        }
        private static class WorkSeat
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
                if (s_allWorkSeats.Count == 0)
                    s_allWorkSeats.AddRange(GameObject.FindGameObjectsWithTag("WorkSeat"));
            }
        }

        private static List<NPCMovement> s_allNPCs = new List<NPCMovement>();
        
        private Vector3 myWorkSeat;
        private NavMeshAgent agentRef;
        private float timeInsideDestination;
        private float workTimeStreak;

        // Delta time
        private float dt;

        private void Awake()
        {
            WorkSeat.InitWorkSeats();
            Toilet.InitToilets();
        }

        void Start()
        {
            // All getcomponent functions are called inside this function, returning false if it fails.
            if (!SetAllGetComponents())
                Debug.LogAssertion("GetComponent Failed inside SetAllGetComponents function.");

            // Add this NPC to the static list of NPCs.
            s_allNPCs.Add(this);

            // Assign this NPCs' work seat.
            Vector3 temp = WorkSeat.s_allWorkSeats[s_allNPCs.IndexOf(this)].transform.position;
            myWorkSeat = new Vector3(temp.x, 0, temp.z);

            // NavMeshAgent starts disabled because Unity has a bug involving it and giving the wrong position.
            agentRef.enabled = true;

            InvokeRepeating("RandomlySetAvoidancePriority", 1, 2);

            // Our NPC starts by going to its work seat.
            agentRef.destination = myWorkSeat;
        }

        void Update()
        {
            //+++ Reduce update calls to 10 times per second.
            dt += Time.deltaTime;
            if (dt >= 0.1f)
            { dt = 0; }
            else
                return;
            //---

            // Random Chance of NPC wanting to go to the toilet.
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

            // Check time an NPC has been inside of its destination (not counting work destination).
            if (agentRef.remainingDistance <= 1f && !IsDestinationMyWorkSeat)
                timeInsideDestination++;

            // Count how long we have been working since interupption.
            if (IsCurrentlyWorking)
                workTimeStreak++;
            else
                workTimeStreak = 0;

            // If our NPC has been at a destination for longer than (5) seconds, go back to work.
            if (timeInsideDestination >= 50)
            {
                agentRef.destination = myWorkSeat;
                timeInsideDestination = 0;
            }
        }

        private void OnDestroy()
        {
            s_allNPCs.Remove(this);
        }

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
        }

        /// <summary>
        /// Returns true if NPC destination is its' work seat.
        /// </summary>
        private bool IsDestinationMyWorkSeat
        {
            get
            {
                if (agentRef.destination.x == myWorkSeat.x && agentRef.destination.z == myWorkSeat.z)
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
                    if (agentRef.remainingDistance <= 1.5f && Vector3.Distance(transform.position, myWorkSeat) < 0.5f)
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
    }
}