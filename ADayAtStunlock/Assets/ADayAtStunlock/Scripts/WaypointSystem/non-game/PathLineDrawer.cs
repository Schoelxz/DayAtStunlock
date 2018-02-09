using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class PathLineDrawer : MonoBehaviour
{

    private List<PathScriptObject> pathList = new List<PathScriptObject>();
    public static List<List<Waypoint>> listOfPathsWithWaypoints = new List<List<Waypoint>>();
    private GameObject folderRef;

    // Use this for initialization
    void Start ()
    {
        Object[] data;
        data = Resources.LoadAll("Waypoint-Paths", typeof(PathScriptObject));

        listOfPathsWithWaypoints.Clear();
        pathList.Clear();
        foreach (PathScriptObject path in data)
        {
            pathList.Add(path);
        }

        folderRef = GameObject.Find("WaypointsFolder");

        for (int i = 0; i < pathList.Count; i++)
        {
            listOfPathsWithWaypoints.Add(new List<Waypoint>());

            for (int j = 0; j < pathList[i].pathWay.Count; j++)
            {

                listOfPathsWithWaypoints[i].Add(folderRef.transform.Find(pathList[i].pathWay[j]).GetComponent<Waypoint>());

            }
        }
	}

    // Update is called once per frame
    void Update ()
    {
        Debug.Log(listOfPathsWithWaypoints);
	}

    //  Better update when it comes to rendering visible stuff in the scene
    void OnRenderObject()
    {
        for (int i = 0; i < listOfPathsWithWaypoints.Count; i++)
        {
            for (int j = 0; j < listOfPathsWithWaypoints[i].Count; j++)
            {
               // int nextPoint = j + 1;
               // if (nextPoint == listOfPathsWithWaypoints[j].Count - 1)
                   // nextPoint = 0;

                //TODO: MAKE NICE LINES HERE. OK?! GOOD!
                Debug.DrawLine(listOfPathsWithWaypoints[i][j].transform.position, listOfPathsWithWaypoints[i][j].transform.position);
            }
        }

        
    }
}
