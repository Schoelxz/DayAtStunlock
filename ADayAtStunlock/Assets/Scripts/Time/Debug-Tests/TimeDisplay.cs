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
            //private Text myText;
            private int minutes;
            private int hours;

            public float timeSpeed = 1;
            public int startMinute = 30;
            public int startHour = 7;

            private Schedule sched;
            private int taskIndex;

            void Start()
            {
                sched = GameObject.FindObjectOfType<Schedule>();
                TimeSystem.TimeMultiplier = timeSpeed;
                TimeSystem.TimePassedSeconds = startMinute + (startHour * 60);
            }

            void Update()
            {
                minutes = ((int)TimeSystem.TimePassedSeconds) % 60;
                hours = ((int)TimeSystem.TimePassedMinutes) % 24;
            }

            void OnGUI()
            {
                taskIndex = sched.myScheduleTasks.IndexOf(sched.myCurrentTask) + 1;
                if (sched.myScheduleTasks.IndexOf(sched.myCurrentTask) + 1 == sched.myScheduleTasks.Count)
                    taskIndex = 1;

                // Make a background box
                GUI.Box(new Rect(10, 10, 200, 40), "Time\n" + hours.ToString("00") + ":" + minutes.ToString("00"));
                GUI.Box(new Rect(10, 50, 200, 40), "Game Seconds Passed\n" + TimeSystem.TimePassedSeconds.ToString());
                GUI.Box(new Rect(10, 90, 200, 40), "GameObject Name\n" + sched.gameObject.name);
                GUI.Box(new Rect(10, 130, 200, 40), sched.myCurrentTask.TaskName + "\nNext task: " + sched.myScheduleTasks[taskIndex].TaskName);
               
                //GUI.Box(new Rect(10, 170, 200, 40), "Next task: " + sched.myScheduleTasks[taskIndex].TaskName);
            }
        }
    } // Namespace DBUG
} // Namespace DAS
