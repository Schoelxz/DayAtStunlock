using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DAS //  Namespace to sort out our own classes from other default classes.
{
    namespace DBUG
    {
        public class TimeDisplay : MonoBehaviour
        {
            private int minutes;
            private int hours;

            public float timeSpeed = 1;
            public int startMinute = 30;
            public int startHour = 7;

            public GameObject NPCObject;
            private GameObject oldNPCObject;
            private Schedule NPCSchedule;
            private int taskIndex;

            void Start()
            {
                TimeSystem.TimeMultiplier = timeSpeed;
                TimeSystem.TimePassedSeconds = startMinute + (startHour * 60);
            }

            void Update()
            {
                if (NPCObject != null && NPCObject != oldNPCObject)
                {
                    NPCSchedule = NPCObject.GetComponent<Schedule>();
                    oldNPCObject = NPCObject;
                }
                minutes = ((int)TimeSystem.TimePassedSeconds) % 60;
                hours = ((int)TimeSystem.TimePassedMinutes) % 24;
            }

            //For drawing GUI to show values and debugging
            void OnGUI()
            {
                if (NPCSchedule != null)
                {
                    taskIndex = NPCSchedule.myScheduleTasks.IndexOf(NPCSchedule.myCurrentTask) + 1;
                    if (NPCSchedule.myScheduleTasks.IndexOf(NPCSchedule.myCurrentTask) + 1 == NPCSchedule.myScheduleTasks.Count)
                        taskIndex = 0;

                    // TimeSpan time = TimeSpan.FromSeconds(sched.myScheduleTasks[taskIndex].StartTime); //
                    TimeSpan tStart = TimeSpan.FromMinutes(NPCSchedule.myScheduleTasks[taskIndex].StartTime);
                    string taskStartTime = string.Format("{0:D2}:{1:D2}", tStart.Hours, tStart.Minutes);
                    ////
                    // Make a background box
                    
                    GUI.Box(new Rect(10, 90, 240, 40), "GameObject Name\n" + NPCSchedule.gameObject.name);
                    GUI.Box(new Rect(10, 130, 240, 40), NPCSchedule.myCurrentTask.TaskName +
                        "\nNext task: " + NPCSchedule.myScheduleTasks[taskIndex].TaskName +
                        " at " + taskStartTime);
                }

                GUI.Box(new Rect(10, 10, 240, 40), "Time\n" + hours.ToString("00") + ":" + minutes.ToString("00"));
                GUI.Box(new Rect(10, 50, 240, 40), "Game Seconds Passed\n" + TimeSystem.TimePassedSeconds.ToString());

                //GUI.Box(new Rect(10, 170, 200, 40), "Next task: " + sched.myScheduleTasks[taskIndex].TaskName);
            }
        }
    } // Namespace DBUG
} // Namespace DAS
