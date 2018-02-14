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

            private Text myText;
            private int minutes;
            private int hours;

            public float timeSpeed = 1;
            public int startMinute = 30;
            public int startHour = 7;

            // Use this for initialization
            void Start()
            {
                myText = GetComponent<Text>();
                TimeSystem.TimeMultiplier = timeSpeed;
                TimeSystem.TimePassedSeconds = startMinute + (startHour * 60);
            }

            // Update is called once per frame
            void Update()
            {
                minutes = ((int)TimeSystem.TimePassedSeconds) % 60;
                hours = ((int)TimeSystem.TimePassedMinutes) % 24;
                myText.text = hours.ToString("00") + ":" + minutes.ToString("00");
            }
        }
    } // Namespace DBUG
} // Namespace DAS
