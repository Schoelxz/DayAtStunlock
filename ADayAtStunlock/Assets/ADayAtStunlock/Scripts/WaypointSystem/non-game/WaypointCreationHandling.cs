#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointCreationHandling : MonoBehaviour
{

    public static List<Waypoint> waypoints = new List<Waypoint>();
    public List<Waypoint> inspectorWaypoints = new List<Waypoint>();

    public GameObject folder;

    // Use this for initialization
    void Start()
    {
        folder = GameObject.Find("WaypointsFolder");
    }

    // Update is called once per frame
    void Update()
    {
        inspectorWaypoints = waypoints;

        if(folder == null)
            folder = GameObject.Find("WaypointsFolder");

        for (int i = 0; i < folder.transform.childCount; i++)
        {
            Waypoint waypoint;
            waypoint = folder.transform.GetChild(i).GetComponent<Waypoint>();
            if (!waypoints.Contains(waypoint))
            {
                waypoints.Add(waypoint);
            }
        }

       // foreach (var waypoint in ) //GetComponentInChildren<Transform>().GetComponent<Waypoint>()
      //  {

       // }

        for (int i = 0; i < waypoints.Count; i++)
        {
            waypoints[i].gameObject.name = "Waypoint " + i;
        }

        Debug.Log("CheckingCreation...");
	}
}
#endif