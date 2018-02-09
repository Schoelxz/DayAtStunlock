using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNavigation : MonoBehaviour
{
    public List<Waypoint> test = new List<Waypoint>();

    public List<List<Waypoint>> listListWaypoints = new List<List<Waypoint>>();

    public bool isToFollowWaypoints = true;

    private IEnumerator coroutineFollowWaypoint;
    public bool coroutineRunning = false;

	// Use this for initialization
	void Start ()
    {
        listListWaypoints.Add(test);
        coroutineFollowWaypoint = FollowWaypoints(listListWaypoints[0]);

    }
    
    // Update is called once per frame
    void Update ()
    {
        if (isToFollowWaypoints && !coroutineRunning)
        {
            coroutineFollowWaypoint = FollowWaypoints(listListWaypoints[0]);
            StartCoroutine(coroutineFollowWaypoint);
            isToFollowWaypoints = false;
        }
    }
    
    IEnumerator FollowWaypoints(List<Waypoint> waypoints)
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
    }
}
