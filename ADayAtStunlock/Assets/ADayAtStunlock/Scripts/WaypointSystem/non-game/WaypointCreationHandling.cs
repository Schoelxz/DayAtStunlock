#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class WaypointCreationHandling : MonoBehaviour
{
    #region Variables
    //  List of all waypoints in the scene.
    public static List<Waypoint> waypoints = new List<Waypoint>();

    #region Deprecated Debug
#pragma warning disable 414  // CS0414 Unused variable.
    //  To show the waypoints inside the inspector where this script lies.
    private List<Waypoint> inspectorWaypoints = new List<Waypoint>();
#pragma warning restore 414
    #endregion

    #region Inspector Variables
    [Header("Text Options")]
    [Range(0.01f, 0.3f)]
    public float textCharacterSize = 0.2f;
    public Color textColor = Color.white;
    [Space]

    [Header("Text Look Direction")]
    [Tooltip("Default text will look at the current camera")]
    public bool textOverrideLookUp = false;
    [Tooltip("Default text will look at the current camera")]
    public bool textLookAtMainCamera = false;
    [Space]

    [Header("Change Waypoint Names")]
    //  List to change names of the waypoints. By chaning the names in this list on the inspector.
    [Tooltip("Change name of waypoint")]
    public List<string> WaypointNames = new List<string>();
    //private static List<string> SavedWaypointNames = new List<string>();
    [Space]

    [SerializeField]
    private GameObject folder; //waypoint folder
    #endregion

    #endregion

    #region Awake/Start
    private void Awake()
    {
        folder = GameObject.Find("WaypointsFolder");
    }

    // Use this for initialization
    void Start()
    {
        FindOrCreateFolder();

        waypoints.Clear();      //   Potential memory leak but will probably get caught by garbage collection
        WaypointNames.Clear(); //   Potential memory leak but will probably get caught by garbage collection
        if (waypoints.Count == 0)
        {
            
            for (int i = 0; i < folder.transform.childCount; i++)
            {
                if (folder.transform.GetChild(i).GetComponent<Waypoint>())
                {
                    waypoints.Add(folder.transform.GetChild(i).GetComponent<Waypoint>());

                    WaypointNames.Add(waypoints[i].gameObject.name);
                }
            }

            inspectorWaypoints = waypoints;
        }
    }
    #endregion

    #region Update
    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            FindOrCreateFolder();

            //  For every waypoint in the scene, we add it to our hierarchy-folder and waypoint list
            //  Unless it already exits in the waypoint list
            foreach (var WP in FindObjectsOfType<Waypoint>())
            {
                if (!waypoints.Contains(WP))
                {
                    WP.transform.parent = folder.transform;
                    waypoints.Add(WP);
                }
            }

            //  Main update for waypoints
            WaypointUpdates();
        }
	}

    //  Better update when it comes to rendering visible stuff in the scene
    void OnRenderObject()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            //  Update waypoint-text editor info.
            if(waypoints[i] != null)
                SetTextMeshOptions(waypoints[i]);
        }
    }

    #endregion

    #region Functions
    /// <summary>
    /// Finds a gameobject in the scene with the name "WaypointsFolder".
    /// Creates it if it cant be found. Then used to store waypoints in the hierarchy.
    /// </summary>
    void FindOrCreateFolder()
    {
        if (folder == null)
        {
            if (!(GameObject.Find("WaypointsFolder")))
            {
                folder = Instantiate(new GameObject());
                folder.gameObject.name = "WaypointsFolder";
            }
            else if (GameObject.Find("WaypointsFolder"))
            {
                folder = GameObject.Find("WaypointsFolder");
            }
        }
    }

    /// <summary>
    /// Updates and sets all textmesh info on the waypoint for the editor.
    /// </summary>
    void SetTextMeshOptions(Waypoint p_waypoint)
    {
        if (!EditorApplication.isPlaying)
        {
            TextMesh tempVar;
            if (p_waypoint.GetComponent<TextMesh>())
            {
                tempVar = p_waypoint.GetComponent<TextMesh>();
            }
            else
            {
                tempVar = p_waypoint.gameObject.AddComponent<TextMesh>();
            }

            tempVar.text = p_waypoint.gameObject.name;
            tempVar.characterSize = textCharacterSize;
            tempVar.anchor = TextAnchor.MiddleCenter;
            tempVar.alignment = TextAlignment.Center;
            tempVar.color = textColor;
        }
    }

    /// <summary>
    /// Updates all waypoints in many different ways...
    /// </summary>
    void WaypointUpdates()
    {
        inspectorWaypoints = waypoints;

        //  Remove unusable names
        while (waypoints.Count < WaypointNames.Count)
        {
            WaypointNames.RemoveAt(WaypointNames.Count - 1);
        }

        //  Check so that no waypoint has the same name
        for (int i = 0; i < WaypointNames.Count; i++)
        {
            for (int j = 0; j < WaypointNames.Count; j++)
            {
                if(WaypointNames[i].ToString() == WaypointNames[j].ToString())
                {
                    if( i != j )
                    {
                        WaypointNames[j] = WaypointNames[j] + " Copy";
                    }
                }
            }
        }

        // Update all waypoints
        for (int i = 0; i < waypoints.Count; i++)
        {
            //  Remove waypoint from list if it has no value.
            if (waypoints[i] == null)
            {
                waypoints.Remove(waypoints[i]);
                WaypointNames.RemoveAt(i);
                inspectorWaypoints = waypoints;
                continue;
            }

            if (!WaypointNames.Contains("Waypoint " + (i + 1)) && WaypointNames.Count < waypoints.Count)
            {
                while (WaypointNames.Count < waypoints.Count)
                {
                    WaypointNames.Add(waypoints[i].gameObject.name = "Waypoint " + (i + 1));
                }
            }
            else
            {
                if (WaypointNames[i] != null)
                {
                    waypoints[i].gameObject.name = WaypointNames[i];
                }
            }

            if (!textOverrideLookUp)
            {
                if (Camera.current != null && !textLookAtMainCamera)
                {
                    waypoints[i].transform.LookAt(Camera.current.transform);
                    waypoints[i].transform.Rotate(0, 180, 0);
                }
                else
                {
                    if (textLookAtMainCamera)
                    {
                        waypoints[i].transform.LookAt(Camera.main.transform);
                        waypoints[i].transform.Rotate(0, 180, 0);
                    }
                }
            }
            else
            {

                waypoints[i].transform.LookAt(waypoints[i].transform.position - Vector3.up);
                    
            }
            

        }
    }
    #endregion
}
#endif