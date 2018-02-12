using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class full of static members and methods,
//  cause it is not gameobject reliant nor should exist on several places
public class WaypointManager : MonoBehaviour
{
    #region public variables
    //Variables used by Waypoint
    public static Dictionary<string, Waypoint> waypointNames = new Dictionary<string, Waypoint>();
    public static List<Waypoint> listOfAllWaypoints = new List<Waypoint>();
    #endregion
}
