﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class full of static members and methods,
//  cause it is not gameobject reliant nor should exist on several places
public class WaypointManager : MonoBehaviour
{

    #region public variables
    //Variables used by Waypoint
    public static Dictionary<string, Waypoint> allWaypointsMap = new Dictionary<string, Waypoint>();
    public static List<Waypoint> listOfAllWaypoints = new List<Waypoint>();
    public static Dictionary<string, PathSO> listOfAllPathsMap = new Dictionary<string, PathSO>();
    #endregion

    private void Start()
    {
        GetAllPaths();
    }

    private void GetAllPaths()
    {
        //  Find and load resources
        Object[] data;
        data = Resources.LoadAll("Waypoint-Paths", typeof(PathSO));

        listOfAllPathsMap.Clear();

        //  Get our scriptable objects from data
        foreach (PathSO path in data)
        {
            if (!listOfAllPathsMap.ContainsValue(path))
                listOfAllPathsMap.Add(path.name, path);
        }
    }

#if UNITY_EDITOR
    #region DebugTesting
    [Header("Debug Testing Helpers")]
    public bool doTheThing = false;
    public bool doTheThingB = false;
    private bool doingTheThing = false;
    public PathSO aPath;
    public GameObject NPC;
    #endregion

    #region DebugTesting
    private void Update()
    {
        if(!doingTheThing && doTheThing)
        {
            doTheThing = false;
            doingTheThing = true;
            NPC.GetComponent<WaypointNavigation>().StartFollowingCurrentRoute(aPath);
        }
        else if (!doingTheThing && doTheThingB)
        {
            doTheThingB = false;
            doingTheThing = true;
            NPC.GetComponent<WaypointNavigation>().StartFollowingCurrentRouteBackwards(aPath);
        }
        else if(NPC != null && !NPC.GetComponent<WaypointNavigation>().CoroutineRunning)
        {
            doingTheThing = false;
        }
    }
    #endregion
#endif
}
