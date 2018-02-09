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
    public static List<Waypoint> testPath = new List<Waypoint>();
    #endregion
    public PathScriptObject path;


/*#if !UNITY_EDITOR
    #region UnityEditorOnly
    void Start ()
    {
        EDITORInspectorList = listOfAllWaypoints;
    }
    #endregion
#else*/

    // Use this for initialization
    void Start()
    {
        /*for (int i = 0; i < path.pathWay.Count; i++)
        {

            testPath.Add(waypointNames[path.pathWay[i]]);

        }*/
    }

//#endif


    #region Update
    // Update is called once per frame
    void Update ()
    {

    }
    #endregion

    #region Functions
    GameObject GetWaypointGameObject(string waypointName)
    {
        return waypointNames[waypointName].gameObject;
    }
    Vector3 GetWaypointPosition(string waypointName)
    {
        return waypointNames[waypointName].transform.position;
    }


    #endregion
}
