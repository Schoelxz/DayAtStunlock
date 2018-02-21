using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Schedule", menuName = "Schedule/Task", order = 1)]
public class TaskObject : ScriptableObject
{
    public float startTime, endTime;
    public string taskName;
}

[CreateAssetMenu(fileName = "Schedule", menuName = "Schedule/Better Task", order = 1)]
public class BetterTaskObject : ScriptableObject
{
    public Vector2[] TaskTime;
    public string[] TaskName;
}

public class Schedule : MonoBehaviour
{
    public struct Task
    {
        private float startTime, endTime;
        private string taskName;

        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public float EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        public string TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }

        public Task(float startTime, float endTime, string taskName)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.taskName = taskName;

            if(this.startTime >= this.endTime)
            {
                // Not a valid event.
            }
        }

    }

    public List<Task> myTasks = new List<Task>();

    public Task myCurrentTask;

	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < myTasks.Count; i++)
        {
            if(myTasks[i].StartTime >= DAS.TimeSystem.TimePassedSeconds && myTasks[i].EndTime < DAS.TimeSystem.TimePassedSeconds)
            {
                myCurrentTask = myTasks[i];
            }
        }
	}

}
