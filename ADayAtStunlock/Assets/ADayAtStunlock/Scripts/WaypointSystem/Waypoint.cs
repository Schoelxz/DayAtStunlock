using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    #region Start
    private void Start()
    {
        Destroy(gameObject.GetComponent<TextMesh>());
        Destroy(gameObject.GetComponent<MeshRenderer>());
        
        if (!WaypointManager.listOfAllWaypoints.Contains(this))
            WaypointManager.listOfAllWaypoints.Add(this);
        
        if (!WaypointManager.waypointNames.ContainsValue(this))
            WaypointManager.waypointNames.Add(gameObject.name, this);

    }
    #endregion

    #region OnDestroy
    private void OnDestroy()
    {

        if (WaypointManager.listOfAllWaypoints.Contains(this))
            WaypointManager.listOfAllWaypoints.Remove(this);

        if (WaypointManager.waypointNames.ContainsValue(this))
            WaypointManager.waypointNames.Remove(gameObject.name);


    }
    #endregion
}

#region OldCodeSaved
/*
[ExecuteInEditMode]
#endif
public class Waypoint : MonoBehaviour
{
    #region UnityEditorOnly
#if UNITY_EDITOR
    #region Editor Variables
    private TextMesh text;
    #endregion
    #region Update
    private void Update()
    {
        if (text != null)
        {
            text.transform.LookAt(Camera.main.transform.position);
            text.transform.Rotate(0, 180, 0);
            text.text = gameObject.name;
        }
    }
    #endregion
#endif
    #endregion

    #region Start
    private void Start()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
#endif
            if (!WaypointManager.listOfAllWaypoints.Contains(this))
                WaypointManager.listOfAllWaypoints.Add(this);

            if (!WaypointManager.waypointNames.ContainsValue(this))
                WaypointManager.waypointNames.Add(gameObject.name, this);
#if UNITY_EDITOR
        }
#endif


        #region UnityEditorOnly
#if UNITY_EDITOR
        transform.parent = GameObject.Find("WaypointsFolder").transform;
        text = gameObject.AddComponent<TextMesh>();
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.characterSize = 0.2f;
#endif
        #endregion
    }
    #endregion

    #region OnDestroy
    private void OnDestroy()
    {
        if (WaypointManager.listOfAllWaypoints.Contains(this))
            WaypointManager.listOfAllWaypoints.Remove(this);

        if (WaypointManager.waypointNames.ContainsValue(this))
            WaypointManager.waypointNames.Remove(gameObject.name);
    }
#endregion
    */
#endregion