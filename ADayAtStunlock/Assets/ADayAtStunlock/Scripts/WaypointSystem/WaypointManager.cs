using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Class full of static members and methods,
//  cause it is not gameobject reliant nor should exist on several places
public class WaypointManager : MonoBehaviour
{
    #region UnityEditorOnly Inspector Variable
#if UNITY_EDITOR

#pragma warning disable 414  // CS0414 Unused variable.
    [SerializeField]
    private List<Waypoint> EDITORInspectorList = new List<Waypoint>();
#pragma warning restore 414
#endif
    #endregion

    #region public variables
    public static Dictionary<string, Waypoint> waypointNames = new Dictionary<string, Waypoint>();
    public static List<Waypoint> listOfAllWaypoints = new List<Waypoint>();
    #endregion

    #region Start
#if UNITY_EDITOR
    #region UnityEditorOnly
    void Start ()
    {
        EDITORInspectorList = listOfAllWaypoints;
    }
    #endregion
#else
    // Use this for initialization
    static void Start()
    {
        
    }
#endif
    #endregion

    #region Update
    // Update is called once per frame
    static void Update ()
    {
        Debug.Log("hello");
    }
    #endregion

    #region Functions
    static GameObject GetWaypointGameObject(string waypointName)
    {
        return waypointNames[waypointName].gameObject;
    }
    static Vector3 GetWaypointPosition(string waypointName)
    {
        return waypointNames[waypointName].transform.position;
    }
    #endregion
}
