using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

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
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.2f;
            text.text = gameObject.name;
        }
        else
        {
            text = gameObject.AddComponent<TextMesh>();
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
            //TODO: Left off from here.... fix destroy on textmesh and mesh renderer....
            Destroy(gameObject.GetComponent<TextMesh>());

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
        if (gameObject.GetComponent<TextMesh>())
            text = FindObjectOfType<TextMesh>();
        else
            text = gameObject.AddComponent<TextMesh>();
#endif
#endregion
    }
#endregion

#region OnDestroy
    private void OnDestroy()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
#endif
            if (WaypointManager.listOfAllWaypoints.Contains(this))
                WaypointManager.listOfAllWaypoints.Remove(this);

            if (WaypointManager.waypointNames.ContainsValue(this))
                WaypointManager.waypointNames.Remove(gameObject.name);
#if UNITY_EDITOR
        }
#endif

        WaypointCreationHandling.waypoints.Remove(this);

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