using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigation : MonoBehaviour
{
    [SerializeField]
    private List<Waypoint> currentRoute = new List<Waypoint>();

    private IEnumerator coroutineFollowWaypoint;
    private IEnumerator coroutineFollowWaypointBackwards;

    [SerializeField]
    private bool coroutineRunning = false;
    [SerializeField]
    private bool isToFollowWaypoints = true;

    void Start ()
    {
    }
    
    void Update ()
    {
    }

    public void StartFollowingCurrentRoute()
    {
        if (isToFollowWaypoints && !coroutineRunning)
        {
            coroutineFollowWaypoint = FollowWaypoints(currentRoute);

            StartCoroutine(coroutineFollowWaypoint);

            isToFollowWaypoints = false;
        }
    }
    public void StartFollowingCurrentRouteBackwards()
    {
        if (isToFollowWaypoints && !coroutineRunning)
        {
            coroutineFollowWaypointBackwards = FollowWaypointsBackwards(currentRoute);

            StartCoroutine(coroutineFollowWaypointBackwards);

            isToFollowWaypoints = false;
        }
    }

    public void SetPathRoute(PathScriptObject path)
    {
        currentRoute.Clear();
        for (int i = 0; i < path.pathWay.Count; i++)
        {
            currentRoute.Add(WaypointManager.allWaypointsMap[path.pathWay[i]]);
        }
    }

    public IEnumerator FollowWaypoints(List<Waypoint> waypoints)
    {
        coroutineRunning = true;
        if (waypoints.Count != 0 && waypoints[0] != null)
        {
            int currentWaypoint = 0;
            bool destinationReached = false;

            while (!destinationReached)
            {
                yield return new WaitForSeconds(0);

                if (currentWaypoint >= waypoints.Count)
                    destinationReached = true;
                else if (!destinationReached)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, 3 * Time.deltaTime);

                    //  if current waypoint is reached
                    if (Mathf.Approximately(transform.position.x, waypoints[currentWaypoint].transform.position.x) &&
                        Mathf.Approximately(transform.position.y, waypoints[currentWaypoint].transform.position.y) &&
                        Mathf.Approximately(transform.position.z, waypoints[currentWaypoint].transform.position.z))
                    {
                        currentWaypoint++;
                    }
                }
                Debug.Log("Coroutine is running...");
            }
        }
        coroutineRunning = false;
        isToFollowWaypoints = true;
    }

    public IEnumerator FollowWaypointsBackwards(List<Waypoint> waypoints)
    {
        coroutineRunning = true;
        if (waypoints.Count != 0 && waypoints[0] != null)
        {
            int currentWaypoint = waypoints.Count - 1;
            bool destinationReached = false;

            while (!destinationReached)
            {
                yield return new WaitForSeconds(0);

                if (currentWaypoint < 0)
                    destinationReached = true;
                else if (!destinationReached)
                {
                    transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, 3 * Time.deltaTime);

                    //  if current waypoint is reached
                    if (Mathf.Approximately(transform.position.x, waypoints[currentWaypoint].transform.position.x) &&
                        Mathf.Approximately(transform.position.y, waypoints[currentWaypoint].transform.position.y) &&
                        Mathf.Approximately(transform.position.z, waypoints[currentWaypoint].transform.position.z))
                    {
                        currentWaypoint--;
                    }
                }
                Debug.Log("Coroutine is running...");
            }
        }
        coroutineRunning = false;
        isToFollowWaypoints = true;
    }
}
