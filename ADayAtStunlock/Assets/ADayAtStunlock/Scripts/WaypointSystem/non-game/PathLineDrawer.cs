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

    [Tooltip("Read only. Cause it does nothing.")]
    public int pathsAvailable;

    public int pathToRender = 0;

    [Header("Line Color Control")]
    [Range(0, 1)]
    public int useRed, useGreen, useBlue;
    



    private float dt; //delta timer to control debug.drawline rendering

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
                if(folderRef.transform.Find(pathList[i].pathWay[j]))
                    listOfPathsWithWaypoints[i].Add(folderRef.transform.Find(pathList[i].pathWay[j]).GetComponent<Waypoint>());
                else
                {
                    Debug.Assert(folderRef.transform.Find(pathList[i].pathWay[j]), "Could not find a waypoint from path: *" + pathList[i].name + "* with waypoint name: *" + pathList[i].pathWay[j] + "*.");
                }

            }
        }
	}

    // Update is called once per frame
    void Update ()
    {

	}



    //  Better update when it comes to rendering visible stuff in the scene
    void OnRenderObject()
    {
        dt += Time.deltaTime;

        pathsAvailable = listOfPathsWithWaypoints.Count;
        for (int i = 0; i < listOfPathsWithWaypoints.Count; i++)
        {
            for (int j = 0; j < listOfPathsWithWaypoints[i].Count; j++)
            {
                int wpIndex = (j+1);
                if (j == listOfPathsWithWaypoints[i].Count - 1)
                {
                    wpIndex = j;
                }

                //  Render pathway
                if (pathToRender == i && dt >= 0.1f)
                {
                    Debug.DrawLine(listOfPathsWithWaypoints[i][j].transform.position,
                        listOfPathsWithWaypoints[i][wpIndex].transform.position,
                        new Color(((1f / listOfPathsWithWaypoints[i].Count) * j) * useRed, (0.2f) * useGreen, (0.2f)* useBlue, 1f), 0.1f);
                }
            }
        }

        if (dt >= 0.1f)
            dt = 0f;

        
        
    }
}
