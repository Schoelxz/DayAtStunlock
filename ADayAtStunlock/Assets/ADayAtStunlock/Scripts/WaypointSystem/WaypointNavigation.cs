﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Developer thought bubble:
 * too many functions?
 * option 1: remove overloaded functions with parameters.
 * option 2: remove SetPathRoute and overloaded functions without parameters.
*/

public class WaypointNavigation : MonoBehaviour
{
    [SerializeField]
    private List<Waypoint> currentRoute = new List<Waypoint>();

    private IEnumerator coroutineFollowWaypoint;
    private IEnumerator coroutineFollowWaypointBackwards;

    [SerializeField]
    private bool coroutineRunning = false;

    /// <summary>
    /// Get if gameobject currently has a coroutine running. Meaning it has not yet reached its' paths' end.
    /// </summary>
    public bool CoroutineRunning
    {
        get { return coroutineRunning; }
    }

    [SerializeField]
    private bool isToFollowWaypoints = true;

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
            currentRoute.Add(WaypointManager.allWaypointsMap[path.pathWay[i]]);
        }
    }

    private IEnumerator FollowWaypoints(List<Waypoint> waypoints)
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

    private IEnumerator FollowWaypointsBackwards(List<Waypoint> waypoints)
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
