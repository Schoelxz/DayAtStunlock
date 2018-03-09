using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Developer thought bubble:
 * too many functions?
 * option 1: remove overloaded functions with parameters.
 * option 2: remove SetPathRoute and overloaded functions without parameters.
 * option 3: let it be.
*/

public class NavWaypointMovement : MonoBehaviour
{
    public Vector3 myWorkSeatPosition;
    public Vector3 myWorkRoomDestination;

    [Range(0.1f, 300f)]
    public float walkSpeed = 3;

    #region private variables
    [SerializeField]
    private List<Waypoint> currentRoute = new List<Waypoint>();

    private IEnumerator coroutineFollowWaypoint;
    private IEnumerator coroutineFollowWaypointBackwards;

    private NavMeshAgent navAgent;

    private const float WAITTIME = 0.1f;

    [SerializeField]
    private bool isToFollowWaypoints = true;
    [SerializeField]
    private bool coroutineRunning = false;
    #endregion

    #region properties
    /// <summary>
    /// Get if gameobject currently has a coroutine running. Meaning it has not yet reached its' paths' end.
    /// </summary>
    public bool CoroutineRunning
    {
        get { return coroutineRunning; }
    }
    #endregion

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        navAgent.speed = walkSpeed; //* DAS.TimeSystem.DeltaTime;
        navAgent.acceleration = walkSpeed * 3; //DAS.TimeSystem.DeltaTime;

        if (coroutineRunning == false)
            navAgent.isStopped = true;
        else
            navAgent.isStopped = false;
    }

    

    #region coroutine functions
    private IEnumerator FollowWaypoints(List<Waypoint> waypoints)
    {
        coroutineRunning = true;
        if (waypoints.Count != 0 && waypoints[0] != null)
        {
            int currentWaypoint = 0;
            bool destinationReached = false;

            while (!destinationReached)
            {
                yield return new WaitForSeconds(WAITTIME);

                if (currentWaypoint >= waypoints.Count)
                    destinationReached = true;
                else if (!destinationReached)
                {

                    navAgent.destination = waypoints[currentWaypoint].transform.position;
                   

                    yield return new WaitForSeconds(WAITTIME);

                    //  if current waypoint is reached
                    if (navAgent.remainingDistance <= 2f)
                    {
                        currentWaypoint++;
                    }
                }
            }
        }
        coroutineRunning = false;
        isToFollowWaypoints = true;
    }
    private IEnumerator FollowWaypoints(List<Vector3> waypoints)
    {
        coroutineRunning = true;
        if (waypoints.Count != 0 && waypoints[0] != null)
        {
            int currentWaypoint = 0;
            bool destinationReached = false;

            while (!destinationReached)
            {
                yield return new WaitForSeconds(WAITTIME);

                if (currentWaypoint >= waypoints.Count)
                    destinationReached = true;
                else if (!destinationReached)
                {

                    navAgent.destination = waypoints[currentWaypoint];


                    yield return new WaitForSeconds(WAITTIME);

                    //  if current waypoint is reached
                    if (navAgent.remainingDistance <= 2f)
                    {
                        currentWaypoint++;
                    }
                }
            }
        }
        coroutineRunning = false;
        isToFollowWaypoints = true;
    }
    private IEnumerator FollowWaypointsBackwards(List<Waypoint> waypoints)
    {
        coroutineRunning = true;
        if (waypoints.Count != 0 && waypoints[0] != null)
        {
            int currentWaypoint = waypoints.Count - 1;
            bool destinationReached = false;

            while (!destinationReached)
            {
                yield return new WaitForSeconds(WAITTIME);

                if (currentWaypoint < 0)
                    destinationReached = true;
                else if (!destinationReached)
                {
                    navAgent.destination = waypoints[currentWaypoint].transform.position;

                    yield return new WaitForSeconds(WAITTIME);

                    //  if current waypoint is reached
                    if (navAgent.remainingDistance <= 0.3f)
                    {
                        currentWaypoint--;
                    }
                }
            }
        }
        coroutineRunning = false;
        isToFollowWaypoints = true;
    }
    #endregion

    #region public functions
    public void GotoMyWorkSeat()
    {
        List<Vector3> workWaypoints = new List<Vector3>();
        if (myWorkRoomDestination == null || myWorkSeatPosition == null || myWorkSeatPosition == Vector3.zero || myWorkRoomDestination == Vector3.zero)
        {
            Debug.LogAssertion("NPCs Work destination or seat position is null. Assign them!");
            return;
        }

        StartCoroutine(FollowWaypoints(workWaypoints));

    }

    /// <summary>
    /// Stops all coroutines. Making this object to stop following whatever path it is currently going to.
    /// </summary>
    public void StopFollowingWaypoints()
    {
        StopAllCoroutines();
        coroutineRunning = false;
        isToFollowWaypoints = true;
    }

    /// <summary>
    /// Starts the coroutine to follow the set path.
    /// </summary>
    public void StartFollowingCurrentRoute()
    {
        if (currentRoute.Count == 0)
        {
            Debug.LogWarning("WaypointNavigation can't find a route for " + gameObject.name + ". Try SetPathRoute first before trying to follow a route!");
        }
        else if (isToFollowWaypoints && !coroutineRunning)
        {
            coroutineFollowWaypoint = FollowWaypoints(currentRoute);

            StartCoroutine(coroutineFollowWaypoint);

            isToFollowWaypoints = false;
        }

    }

    /// <summary>
    /// Starts the coroutine to follow the set path.
    /// </summary>
    /// <param name="path">path is a scriptable object containing a list of waypoint names.</param>
    public void StartFollowingCurrentRoute(PathSO path)
    {
        currentRoute.Clear();
        for (int i = 0; i < path.pathWay.Count; i++)
        {
            currentRoute.Add(WaypointManager.allWaypointsMap[path.pathWay[i]]);
        }
        if (currentRoute.Count == 0)
        {
            Debug.LogWarning("WaypointNavigation can't find a route for " + gameObject.name + ". Try SetPathRoute first before trying to follow a route!");
        }
        else if (isToFollowWaypoints && !coroutineRunning)
        {
            coroutineFollowWaypoint = FollowWaypoints(currentRoute);

            StartCoroutine(coroutineFollowWaypoint);

            isToFollowWaypoints = false;
        }

    }

    /// <summary>
    /// Starts the coroutine to follow the set path, backwardly.
    /// </summary>
    public void StartFollowingCurrentRouteBackwards()
    {
        if (currentRoute.Count == 0)
        {
            Debug.LogWarning("WaypointNavigation can't find a route for " + gameObject.name + ". Try SetPathRoute first before trying to follow a route!");
        }
        else if (isToFollowWaypoints && !coroutineRunning)
        {
            coroutineFollowWaypointBackwards = FollowWaypointsBackwards(currentRoute);

            StartCoroutine(coroutineFollowWaypointBackwards);

            isToFollowWaypoints = false;
        }
    }

    /// <summary>
    /// Starts the coroutine to follow the set path, backwardly.
    /// </summary>
    /// <param name="path">path is a scriptable object containing a list of waypoint names.</param>
    public void StartFollowingCurrentRouteBackwards(PathSO path)
    {
        currentRoute.Clear();
        for (int i = 0; i < path.pathWay.Count; i++)
        {
            currentRoute.Add(WaypointManager.allWaypointsMap[path.pathWay[i]]);
        }
        if (currentRoute.Count == 0)
        {
            Debug.LogWarning("WaypointNavigation can't find a route for " + gameObject.name + ". Try SetPathRoute first before trying to follow a route!");
        }
        else if (isToFollowWaypoints && !coroutineRunning)
        {
            coroutineFollowWaypointBackwards = FollowWaypointsBackwards(currentRoute);

            StartCoroutine(coroutineFollowWaypointBackwards);

            isToFollowWaypoints = false;
        }
    }

    /// <summary>
    /// Sets what path the gameobject will take.
    /// </summary>
    /// <param name="path">path is a scriptable object containing a list of waypoint names.</param>
    public void SetPathRoute(PathSO path)
    {
        currentRoute.Clear();
        for (int i = 0; i < path.pathWay.Count; i++)
        {
            if (WaypointManager.allWaypointsMap.ContainsKey(path.pathWay[i]))
                currentRoute.Add(WaypointManager.allWaypointsMap[path.pathWay[i]]);
        }
    }
    #endregion
}