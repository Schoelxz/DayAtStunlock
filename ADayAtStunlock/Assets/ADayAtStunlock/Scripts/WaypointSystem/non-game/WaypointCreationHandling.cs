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
    public static List<Waypoint> waypoints = new List<Waypoint>(); //Waypoints are automatically removed from list through waypoints object itself.

    public int waypointNamesCount;


    private int numberOfWaypointsChecker = 0;

    #region Deprecated Debug
#pragma warning disable 414  // CS0414 Unused variable.
    //  To show the waypoints inside the inspector where this script lies.
    [Header("Debug inspector waypoints (read only plz)")]
    public List<Waypoint> inspectorWaypoints = new List<Waypoint>();
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
    public static Dictionary<Waypoint, string> WaypointNames = new Dictionary<Waypoint, string>();

    //private static List<string> SavedWaypointNames = new List<string>();
    [Space]

    //[SerializeField]
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
    }
    #endregion

    #region Update
    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            FindOrCreateFolder();
        }
	}

    //  Better update when it comes to rendering visible stuff in the scene
    void OnRenderObject()
    {
        if (!EditorApplication.isPlaying)
        {
            //  For every waypoint in the scene, we add it to our hierarchy-folder
            //  Unless it already exits in the waypoint list
            PutAllWaypointsInsideFolder();

            //waypoints.Clear();

            if(numberOfWaypointsChecker != waypoints.Count)
            {

            }

            //  Add all the waypoints inside the folder to the waypoints list
            //  Unless it already exits in the waypoint list
            AddAllWaypointsToList();

            //  Add the waypoint names to the waypoint names list
            //  Unless it already exits in the waypoint names list
            AddNameOfWaypointsToList();

            SetGameobjectsName();

            waypointNamesCount = WaypointNames.Count;

            //  Main update for waypoints
            WaypointUpdates();


            SetTextMeshOptions();
            
        }
    }
    #endregion

    #region Functions


    /// <summary>
    /// Updates all waypoints in many different ways...
    /// </summary>
    void WaypointUpdates()
    {
        inspectorWaypoints = waypoints;



        // Update all waypoints
        for (int i = 0; i < waypoints.Count; i++)
        {
            //  Remove waypoint from list if it has no value.
            if (waypoints[i] == null)
            {
                Debug.LogAssertion("waypoint is null...");
                continue;
            }

            // Update rotation. textmesh looking direction
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

    /// <summary>
    /// Finds a gameobject in the scene with the name "WaypointsFolder".
    /// Creates it if it cant be found. Then used to store waypoints in the hierarchy.
    /// Order 1
    /// </summary>
    void FindOrCreateFolder()
    {
        if (folder == null)
        {
            if (!(GameObject.Find("WaypointsFolder")))
            {
                folder = Instantiate(new GameObject());
                folder.gameObject.name = "WaypointsFolder";
                Debug.Log("Created Waypoints Folder");
            }
            else if (GameObject.Find("WaypointsFolder"))
            {
                folder = GameObject.Find("WaypointsFolder");
            }
        }
    }

    /// <summary>
    /// Puts all waypoints currently not inside the folder, inside the folder.
    /// Order 2
    /// </summary>
    void PutAllWaypointsInsideFolder()
    {
        foreach (var WP in FindObjectsOfType<Waypoint>())
        {
            if (!waypoints.Contains(WP))
            {
                WP.transform.parent = folder.transform;
            }
        }
    }

    /// <summary>
    /// Adds all waypoints currently inside the folder to our waypoints list
    /// Order 3
    /// </summary>
    void AddAllWaypointsToList()
    {
        foreach (Transform WP in folder.transform)
        {
            if (!waypoints.Contains(WP.GetComponent<Waypoint>()))
                waypoints.Add(WP.GetComponent<Waypoint>());
        }
    }

    /// <summary>
    /// Adds all waypoints gameobject names to a ´dictionary of string values
    /// Order 4
    /// </summary>
    void AddNameOfWaypointsToList()
    {
        if (waypoints != null && waypoints.Count != 0)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                if (!WaypointNames.ContainsKey(waypoints[i]))
                {
                    WaypointNames.Add(waypoints[i], "Waypoint " + (i + 1));
                    waypoints[i].waypointName = WaypointNames[waypoints[i]];
                }
            }

            //  Check so that no waypoint has the same name
            for (int i = 0; i < waypoints.Count; i++)
            {
                for (int j = 0; j < waypoints.Count; j++)
                {
                    // if these two waypoints has the same name
                    if (waypoints[i].waypointName == waypoints[j].waypointName)
                    {
                        // if its not the same waypoint
                        if (i != j)
                        {
                            WaypointNames[waypoints[j]] = WaypointNames[waypoints[j]] + " Copy";
                            waypoints[j].waypointName = WaypointNames[waypoints[j]];
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets all waypoint gameobjects name to their waypoint name
    /// Order 5
    /// </summary>
    void SetGameobjectsName()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            waypoints[i].gameObject.name = waypoints[i].waypointName;
        }
    }

    /// <summary>
    /// Updates and sets all textmesh info on the waypoint for the editor.
    /// Order 6
    /// </summary>
    void SetTextMeshOptions()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            //  Update waypoint-text editor info.
            if (waypoints[i] != null)
            {
                if (!EditorApplication.isPlaying)
                {
                    TextMesh tempVar;
                    if (waypoints[i].GetComponent<TextMesh>())
                    {
                        tempVar = waypoints[i].GetComponent<TextMesh>();
                    }
                    else
                    {
                        tempVar = waypoints[i].gameObject.AddComponent<TextMesh>();
                    }

                    tempVar.text = waypoints[i].gameObject.name;
                    tempVar.characterSize = textCharacterSize;
                    tempVar.anchor = TextAnchor.MiddleCenter;
                    tempVar.alignment = TextAlignment.Center;
                    tempVar.color = textColor;
                }
            }
        }
    }


    #endregion
}
#endif
