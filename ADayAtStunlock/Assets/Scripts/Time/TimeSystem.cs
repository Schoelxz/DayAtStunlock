using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    #region Time Properties
    private static float timeMultiplier = 1;
    /// <summary>
    /// Changes game speed by multiplying the delta time.
    /// Value is clamped between 0.1 and 4.
    /// </summary>
    public static float TimeMultiplier
    {
        get { return timeMultiplier; }
        set { timeMultiplier = Mathf.Clamp(value, 0.1f, 4f); }
    }

    private static float deltaTime;
    /// <summary>
    /// Get delta time times a multiplier used for controlling speed.
    /// </summary>
    public static float DeltaTime
    {
        get { return deltaTime; }
        private set { deltaTime = value; }
    }

    private static float timePassedSeconds;
    /// <summary>
    /// Get total time passed in seconds since game start(affected by time multiplier).
    /// </summary>
    public static float TimePassedSeconds
    {
        get { return timePassedSeconds; }
        private set { timePassedSeconds = value; } //Time should not be changed from outside sources, therefore private.
    }
    /// <summary>
    /// Get total time passed in minutes since game start(affected by time multiplier).
    /// </summary>
    public static float TimePassedMinutes
    {
        get { return TimePassedSeconds / 60; }
    }
    /// <summary>
    /// Get total time passed in hours since game start(affected by time multiplier).
    /// </summary>
    public static float TimePassedHours
    {
        get { return TimePassedMinutes / 60; }
    }

    private static float realTimePassedSeconds;
    /// <summary>
    /// Get total real time passed in seconds since game start (unaffected by time multiplier).
    /// </summary>
    public static float RealTimePassedSeconds
    {
        get { return realTimePassedSeconds; }
        private set { realTimePassedSeconds = value; } //Time should not be changed from outside sources, therefore private.
    }
    #endregion

    // Update is called once per frame
    private void Update()
    {
        //Update our time.
        UpdateAllTimeVariables();
    }

    /// <summary>
    /// Called in Update to keep variables updated. Updates game specific delta time, time passed in seconds and real time passed in seconds.
    /// </summary>
    private static void UpdateAllTimeVariables() //Private cause only this script needs to keep track of updating it. Unless this class would go static at some point and update no longer works inside of this script.
    {
        DeltaTime = Time.deltaTime * TimeMultiplier; //Game delta time
        RealTimePassedSeconds += Time.deltaTime; //Real seconds
        TimePassedSeconds += DeltaTime; //Game seconds
    }

}
