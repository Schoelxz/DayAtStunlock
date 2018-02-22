using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Schedule : MonoBehaviour
{
    #region Struct: Task
    // Disables warning for more overloads
    #pragma warning disable 0660, 0661
    public struct Task
    {
        // I need to overload the == and != operators to compare this struct with same type of struct
        public static bool operator ==(Task left, Task right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Task left, Task right)
        {
            return !left.Equals(right);
        }

    #pragma warning restore 0660, 0661
    // Restored warning

        // Variables
        private float startTime, endTime;
        private string taskName;

        // Properties
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
        public Vector2 TimeRange
        {
            get { return new Vector2(startTime, endTime); }
            set { startTime = value.x; endTime = value.y; }
        }

        // Constructors
        public Task(float startTime, float endTime, string taskName)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.taskName = taskName;

            if (this.startTime >= this.endTime)
            {
                // Not a valid event.
                Debug.Assert(false, "A Tasks' starting time can't be bigger or equal to it's end time! timestart: " + this.startTime + ". timeend: " + this.endTime);
            }
        }
        public Task(Vector2 timeRange, string taskName)
        {
            this.startTime = timeRange.x;
            this.endTime = timeRange.y;
            this.taskName = taskName;

            if (this.startTime >= this.endTime)
            {
                // Not a valid event.
                Debug.Assert(false, "A Tasks' starting time can't be bigger or equal to it's end time! timestart: " + this.startTime + ". timeend: " + this.endTime);
            }
        }
    }
    #endregion

    public List<Task> myTasks = new List<Task>();
    public Task myCurrentTask;

    private Task oldTask;
    private int taskIndex = 0;

    private void Start()
    {
        oldTask = myCurrentTask;
        SetDefaultTasks();
    }

    void Update ()
    {
        SetCurrentTask();
        
        OnCurrentTaskChanges();
        Debug.Log(myCurrentTask.TaskName);
	}

    private void SetCurrentTask()
    {
        if ((myTasks[myTasks.Count - 1].EndTime > (DAS.TimeSystem.TimePassedSeconds % 1440f) && myTasks[0].StartTime <= (DAS.TimeSystem.TimePassedSeconds % 1440f)))
        {
            if (taskIndex == myTasks.Count)
            {
                myCurrentTask = new Task();
                taskIndex = 0;
            }

            for (int i = taskIndex; i < myTasks.Count; i++)
            {
                if ((DAS.TimeSystem.TimePassedSeconds % 1440f) >= myTasks[i].StartTime && (DAS.TimeSystem.TimePassedSeconds % 1440f) < myTasks[i].EndTime)
                {
                    ++taskIndex;
                    myCurrentTask = myTasks[i];
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Sort of a listener when current task is changed. Is called in update to "listen" for change.
    /// </summary>
    void OnCurrentTaskChanges()
    {
        // Runs code once when current task != old task
        if(myCurrentTask != oldTask)
        {
            oldTask = myCurrentTask;
            // Put code under here:...
            
        }
    }

    /// <summary>
    /// Sets a default schedule of: -> arriving to work -> work -> lunch -> work -> leave work ->
    /// </summary>
    void SetDefaultTasks()
    {
        Object[] taskData;
        taskData = Resources.LoadAll("Schedule", typeof(BetterTaskObject));

        //Uncomment this when you want to get thwe data back!.
        Debug.Log(Resources.Load("Schedule/DefaultSchedule"));

        BetterTaskObject defaultTask = null;

        foreach (BetterTaskObject item in taskData)
        {
            defaultTask = item;
        }
        
        for (int i = 0; i < defaultTask.TaskTime.Length; i++)
        {
            myTasks.Add(new Task(defaultTask.TaskTime[i], defaultTask.TaskName[i]));
        }
    }

}
