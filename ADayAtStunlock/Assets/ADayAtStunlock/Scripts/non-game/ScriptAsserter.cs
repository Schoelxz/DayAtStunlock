// Create a non-MonoBehaviour class which displays
// messages when a game is loaded.
// But right now only in the editor.

#if UNITY_EDITOR
using UnityEngine;

class ScriptAsserter
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("ScriptAsserter Checking scripts...");

        #region WaypointManager
        Debug.Assert((MonoBehaviour.FindObjectOfType<WaypointManager>()), "Warning: No WaypointManager exists. Add it to the *Game Manager* gameobject inside the scene");

        Debug.Assert(!(MonoBehaviour.FindObjectsOfType<WaypointManager>().Length > 1), "More than one of the WaypointManager scripts exists, only one should exist!");
        #endregion
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnSecondRuntimeMethodLoad()
    {
        //Debug.Log("SecondMethod After scene is loaded and game is running.");
    }
}
#endif