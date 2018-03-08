// Create a non-MonoBehaviour class which displays
// messages when a game is loaded.
// But right now only in the editor.

#if UNITY_EDITOR
using UnityEngine;

static class ScriptAsserter
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("ScriptAsserter Checking scripts...");

        #region GameObjects
        Debug.Assert(GameObject.Find("EditorManager"), "Warning: No EditorManager gameobject found. Please create it and put relevant scripts on it");

        Debug.Assert(GameObject.Find("GameManager"), "Warning: No GameManager gameobject found. Please create it and put relevant scripts on it");
        #endregion

        #region WaypointManager
        Debug.Assert((MonoBehaviour.FindObjectOfType<WaypointManager>()), "Warning: No WaypointManager exists. Add it to the Game Manager gameobject inside the scene");

        Debug.Assert(!(MonoBehaviour.FindObjectsOfType<WaypointManager>().Length > 1), "More than one of the WaypointManager scripts exists, only one should exist!");
        #endregion

        #region WaypointCreationHandling
        Debug.Assert(MonoBehaviour.FindObjectOfType<WaypointCreationHandling>(), "Warning: No WaypointCreationHandling script exists. Add it to the EditorManager gameobject.");

        Debug.Assert(!(MonoBehaviour.FindObjectsOfType<WaypointCreationHandling>().Length > 1), "Warning: More than one of WaypointCreationHandling scripts exists, only one should exist!.");
        #endregion
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnSecondRuntimeMethodLoad()
    {
        //Debug.Log("SecondMethod After scene is loaded and game is running.");
    }
}
#endif