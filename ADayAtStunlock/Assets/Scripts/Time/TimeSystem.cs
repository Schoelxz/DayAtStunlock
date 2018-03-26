using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAS //  Namespace to sort out our own classes from other default classes.
{
    /// <summary>
    /// TimeSystem a Singleton Class, for all your timey needs. Provides properties for deltatime, getting time passed and controlling game speed.
    /// <para>Should exist on a GameManager</para>
    /// </summary>
    public class TimeSystem : MonoBehaviour
    {
        //Singleton class
        private static TimeSystem instance = null; //Private cause class only has static variables to reference to, no need to be public.

        #region Time Properties
        private static bool isGamePaused;

        private static float timeMultiplier = 1;
        /// <summary>
        /// Changes game speed by multiplying the delta time.
        /// Value is clamped between 0.1 and 20.
        /// </summary>
        public static float TimeMultiplier
        {
            get { return timeMultiplier; }
            set
            {
                timeMultiplier = Mathf.Clamp(value, 0.1f, 100f);

                //Editor only
#if UNITY_EDITOR
                if (timeMultiplier == value)
                    Debug.Log("Time Multiplier was set to " + timeMultiplier + " (" + value + ")");
                else
                    Debug.Log("Time Multiplier tried to be set to " + value + " but was clamped to " + timeMultiplier);
#endif
            }
        }

        private static float deltaTime;
        /// <summary>
        /// Get delta time times a multiplier used for controlling speed.
        /// If real time reliant then use Time.deltaTime instead .
        /// </summary>
        public static float DeltaTime
        {
            get { return deltaTime; }
            private set { deltaTime = value; } //DeltaTime should not be changed from outside sources, therefore private.
        }

        private static float timePassedSeconds;
        /// <summary>
        /// Get total time passed in seconds since game start(affected by time multiplier).
        /// Is never negative, always returns and sets an absolute number.
        /// </summary>
        public static float TimePassedSeconds
        {
            get { return Mathf.Abs(timePassedSeconds); }
            set { timePassedSeconds = Mathf.Abs(value); }
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

        public static bool IsGamePaused
        {
            get { return isGamePaused; }
        }
        #endregion

        private void Awake()
        {
            //Singleton behavior
            if (instance == null)
                instance = this;
            else
            {
                if(gameObject.name != "UI - Menus")
                    Debug.LogWarning("Another TimeSystem script already exists, destroying the new one inside of gameobject " + gameObject.name + ". Don't forget to remove me after play!");
                Destroy(this);
            }
        }

        private void Update()
        {
            //Update our time properties.
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
       
        /// <summary>
        /// TimePassedSeconds = 0
        /// Time Multiplier = 1;
        /// </summary>
        public static void ResetTime()
        {
            TimePassedSeconds = 0;
            timeMultiplier = 1;
        }
        public static void PauseTime()
        {
            //m_previousTimeMultiplier = timeMultiplier;
            //timeMultiplier = 0;

            // Time Scale fixes EVERYTHING (and thats kinda nice AND annoying).
            Time.timeScale = 0;
            isGamePaused = true;
        }
        public static void ResumeTime()
        {
            // Time Scale fixes EVERYTHING (and thats kinda nice AND annoying).
            Time.timeScale = 1;
            isGamePaused = false;

            //timeMultiplier = m_previousTimeMultiplier;
        }
    }

}// Namespace DAS