﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PathLineDrawer : MonoBehaviour
{
    private List<PathScriptObject> pathList = new List<PathScriptObject>();
    public static List<List<Waypoint>> listOfPathsWithWaypoints = new List<List<Waypoint>>();
    private GameObject folderRef;

    [Header("Path Render Control")]
    [Tooltip("Read only. Cause it does nothing.")]
    public int pathsAvailable;

    [Tooltip("First path renders at zero.")]
    public int pathToRender = 0;
    [Space]

    [Header("Line Color Control")]
    public Color lineColor;
    [Range(0, 1)]
    public int useRed = 1;
    [Range(0, 1)]
    public int useGreen = 1;
    [Range(0, 1)]
    public int useBlue = 0;

    private float dt; //delta timer to control debug.drawline rendering

    // Use this for initialization
    void Start ()
    {
        Object[] data;
        data = Resources.LoadAll("Waypoint-Paths", typeof(PathScriptObject));

        listOfPathsWithWaypoints.Clear();
        // pathList.Clear();
        foreach (PathScriptObject path in data)
        {
            if(!pathList.Contains(path))
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
                    Debug.Assert(folderRef.transform.Find(pathList[i].pathWay[j]),
                        "Could not find a waypoint from path: *" + pathList[i].name +
                        "* with waypoint name: *" + pathList[i].pathWay[j] + "*.");
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
                //  Check if waypoints is null
                if (listOfPathsWithWaypoints[i][j] == null)
                {
                    Debug.LogWarning("Waypoint is null inside listOfPathsWithWaypoints at i = " + i + " and j = " + j);
                    continue;
                }

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
                        new Color(
                        ((lineColor.r / listOfPathsWithWaypoints[i].Count) * j) * Mathf.Clamp(useRed,    0, 1),
                        ((lineColor.g / listOfPathsWithWaypoints[i].Count) * j) * Mathf.Clamp(useGreen,  0, 1),
                        ((lineColor.b / listOfPathsWithWaypoints[i].Count) * j) * Mathf.Clamp(useBlue,   0, 1), 1f), Mathf.Clamp(lineColor.a, 0.1f, 1f));
                } 
            }
        }

        if (dt >= 0.1f)
            dt = 0f;
    }
}
