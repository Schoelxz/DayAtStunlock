using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DAS
{
    public class NavMovementSimple : MonoBehaviour
    {
        public GameObject myWorkSeat;
        public GameObject[] myWorkDestination = new GameObject[2];

        private NavMeshAgent agentRef;

        // Use this for initialization
        void Start()
        {
            agentRef = GetComponent<NavMeshAgent>();
            StartCoroutine(GotoMyWorkPlace(myWorkSeat));
        }

        public void GotoMyWorkPlace()
        {
            if (myWorkSeat == null || myWorkDestination == null)
            {
                return;
            }
            List<GameObject> workplaceWaypoints = new List<GameObject>();

            workplaceWaypoints.AddRange(myWorkDestination);
            workplaceWaypoints.Add(myWorkSeat);

            StartCoroutine(GotoMyWorkPlace(workplaceWaypoints));
        }
        /// <summary>
        /// Coroutine function.
        /// </summary>
        private IEnumerator GotoMyWorkPlace(List<GameObject> waypoints)
        {
            int currentWaypoint = 0;
            bool destinationReached = false;

            while (!destinationReached)
            {
                yield return new WaitForSeconds(0.02f);

                if (currentWaypoint >= waypoints.Count)
                    destinationReached = true;
                else if (!destinationReached)
                {
                    agentRef.destination = waypoints[currentWaypoint].transform.position;

                    //  if current waypoint is reached
                    if (agentRef.remainingDistance <= 2f)
                    {
                        currentWaypoint++;
                    }
                }
            }
        }
        /// <summary>
        /// Coroutine function.
        /// </summary>
        private IEnumerator GotoMyWorkPlace(GameObject waypoint)
        {
            bool destinationReached = false;

            while (!destinationReached)
            {
                yield return new WaitForSeconds(0.02f);

                if (!destinationReached)
                {
                    agentRef.destination = waypoint.transform.position;

                    //  if current waypoint is reached
                    if (agentRef.remainingDistance <= 2f)
                    {
                        destinationReached = true;
                    }
                }
            }
        }
    }
}