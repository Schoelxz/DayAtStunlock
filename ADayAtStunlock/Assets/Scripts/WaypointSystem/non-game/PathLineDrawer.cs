#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DAS
{
    namespace DBUG
    {
        [ExecuteInEditMode]
        public class PathLineDrawer : MonoBehaviour
        {
            #region Variables
            [Header("")]
            private List<PathSO> pathList = new List<PathSO>();
            public static List<List<Waypoint>> listOfPathsWithWaypoints = new List<List<Waypoint>>();
            private GameObject folderRef;

            [Header("Start play mode and end it to update Paths!")]
            [Tooltip("Read only. Cause it does nothing.")]
            public int pathsAvailable;

            [Header("Path Render Control")]
            [Tooltip("First path renders at zero.")]
            public int pathToRender = 0;
            [Space]

            [Header("Line Color Control")]
            public Color lineColor;

            public GameObject start, end;

            private float dt; //delta timer to control debug.drawline rendering
            #endregion

            void Start()
            {
                if (start != null)
                {
                    start.GetComponent<TextMesh>().characterSize = 1f;
                    start.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                    start.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                    start.GetComponent<TextMesh>().color = Color.green;
                }
                if (end != null)
                {
                    end.GetComponent<TextMesh>().characterSize = 1f;
                    end.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
                    end.GetComponent<TextMesh>().alignment = TextAlignment.Center;
                    end.GetComponent<TextMesh>().color = Color.red;
                }

                //  Find and load resources
                Object[] data;
                data = Resources.LoadAll("Waypoint-Paths", typeof(PathSO));

                listOfPathsWithWaypoints.Clear();

                //  Get our scriptable objects from data
                foreach (PathSO path in data)
                {
                    if (!pathList.Contains(path))
                        pathList.Add(path);
                }

                folderRef = GameObject.Find("WaypointsFolder");

                for (int i = 0; i < pathList.Count; i++)
                {
                    listOfPathsWithWaypoints.Add(new List<Waypoint>());

                    for (int j = 0; j < pathList[i].pathWay.Count; j++)
                    {
                        if (folderRef.transform.Find(pathList[i].pathWay[j]))
                            listOfPathsWithWaypoints[i].Add(folderRef.transform.Find(pathList[i].pathWay[j]).GetComponent<Waypoint>());
                        else
                        {
                            Debug.LogWarning("Could not find a waypoint from path: *" + pathList[i].name +
                                "* with waypoint name: *" + pathList[i].pathWay[j] + "*.");
                        }
                    }
                }
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
                        if (listOfPathsWithWaypoints[i][j] == null || listOfPathsWithWaypoints[i] == null)
                        {
                            //Debug.LogWarning("Waypoint is null inside listOfPathsWithWaypoints at i = " + i + " and j = " + j);
                            continue;
                        }

                        // Next waypoint to know what line to draw TO. FROM we already have in j.
                        int wpIndex = (j + 1);
                        if (j == listOfPathsWithWaypoints[i].Count - 1)
                        { wpIndex = j; }

                        //  Render pathway
                        if (pathToRender == i && dt >= 0.1f && listOfPathsWithWaypoints[i][wpIndex] != null)
                        {
                            if (j == 0) // Draw a green line at the start of the path
                            {
                                if (start != null)
                                    start.transform.position = listOfPathsWithWaypoints[i][j].transform.position;
                                Debug.DrawLine(listOfPathsWithWaypoints[i][j].transform.position,
                                listOfPathsWithWaypoints[i][wpIndex].transform.position, new Color(0, 1, 0), 0.2f);
                            }
                            else if (j == listOfPathsWithWaypoints[i].Count - 2) // Draw a red line at the end of the path
                            {
                                if (end != null)
                                    end.transform.position = listOfPathsWithWaypoints[i][j + 1].transform.position;
                                Debug.DrawLine(listOfPathsWithWaypoints[i][j].transform.position,
                                listOfPathsWithWaypoints[i][wpIndex].transform.position, new Color(1, 0, 0), 0.2f);
                            }
                            else // Draw every other line in a custom color, starting from dark, going brighter the closer to the end!
                            {
                                Debug.DrawLine(
                                listOfPathsWithWaypoints[i][j].transform.position,
                                listOfPathsWithWaypoints[i][wpIndex].transform.position,
                                new Color(
                                ((lineColor.r / listOfPathsWithWaypoints[i].Count) * j), //Red
                                ((lineColor.g / listOfPathsWithWaypoints[i].Count) * j), //Green
                                ((lineColor.b / listOfPathsWithWaypoints[i].Count) * j), //Blue
                                1f), 0.2f);                                                 //Alpha
                            }
                        }
                    }
                }

                if (dt >= 0.1f)
                    dt = 0f;
            }
        }
    }
}
#endif