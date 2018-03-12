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

        void Start()
        {
            agentRef = GetComponent<NavMeshAgent>();
            StartCoroutine(GotoMyWorkPlace(myWorkSeat));
        }

        /// <summary>
        /// Stops the current order where the NPC is going and stops it until a new goto is called.
        /// </summary>
        public void StopAllGoto()
        {
            StopAllCoroutines();
            agentRef.destination = transform.position;
            agentRef.isStopped = true;
        }

        public void GotoSelectedLocation(Vector3 location)
        {
            agentRef.destination = location;
            agentRef.isStopped = false;
        }

        /// <summary>
        /// Makes NPC with this script on it to walk to its workplace (with workdestination waypoints for fine-tune in the end?)
        /// </summary>
        public void GotoMyWorkPlace()
        {
            if (myWorkSeat == null || myWorkDestination == null)
            {
                return;
            }
            List<GameObject> workplaceWaypoints = new List<GameObject>();

            workplaceWaypoints.AddRange(myWorkDestination);
            workplaceWaypoints.Add(myWorkSeat);

            StartCoroutine(GotoWaypointsInOrder(workplaceWaypoints));

        }

        /// <summary>
        /// Coroutine function.
        /// </summary>
        private IEnumerator GotoWaypointsInOrder(List<GameObject> waypoints)
        {
            agentRef.isStopped = false;
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
            agentRef.isStopped = false;
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