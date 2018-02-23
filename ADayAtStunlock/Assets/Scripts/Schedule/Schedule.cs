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

    public List<Task> myScheduleTasks = new List<Task>();
    public Task myCurrentTask;

    private Task oldTask;
    private int taskIndex = 0;

    private void Start()
    {
        oldTask = myCurrentTask;
        myScheduleTasks = GetDefaultScheduleTasks();
        StartCoroutine(CorOnCurrentTaskChanges());
        ScheduleManager.AllSchedules.Add(this);
        ScheduleManager.CheckConflictingSchedule();
    }

    private void Update ()
    {
        SetCurrentTask();
        
       // OnCurrentTaskChanges();
	}

    /// <summary>
    /// Sets myCurrentTask to the current task inside myScheduleTasks.
    /// </summary>
    private void SetCurrentTask()
    {
        if(myScheduleTasks == null)
        {
            Debug.Assert(myScheduleTasks.Count == 0, "Schedule: List of tasks is empty.");
            return;
        }

        // if time has passed over the last tasks' end time, and, the time is below the first tasks' start time..
        if ((myScheduleTasks[myScheduleTasks.Count - 1].EndTime > (DAS.TimeSystem.TimePassedSeconds % 1440f) &&
            myScheduleTasks[0].StartTime <= (DAS.TimeSystem.TimePassedSeconds % 1440f)))
        {
            // for each task, check if the current indexed task is active and set it to "my current task"
            for (int i = taskIndex; i < myScheduleTasks.Count; i++)
            {
                // if time is within a tasks' time range.
                if ((DAS.TimeSystem.TimePassedSeconds % 1440f) >= myScheduleTasks[i].StartTime &&
                    (DAS.TimeSystem.TimePassedSeconds % 1440f) < myScheduleTasks[i].EndTime)
                {
                    // if our current task is already set.
                    if (myCurrentTask != myScheduleTasks[i])
                    {
                        ++taskIndex;
                        myCurrentTask = myScheduleTasks[i];
                    }
                    break; // break forloop for we have reached our goal.
                }
            }
        }
        else // else if time is not within the time range of the first and last task..
        {
            // if current task index is the last index..
            if (taskIndex == myScheduleTasks.Count)
            {
                // set current task to null since we have none
                myCurrentTask = new Task();
                // restart the taskindex to the first task index
                taskIndex = 0;
            }
        }
    }

    /// <summary>
    /// Sort of a listener when current task is changed. Is called in update to "listen" for change.
    /// Currently only used for debug logging our current task.
    /// </summary>
    private void OnCurrentTaskChanges()
    {
        // Runs code once when current task != old task
        if(myCurrentTask != oldTask)
        {
            oldTask = myCurrentTask;
            // Put code under here:...
            Debug.Log(myCurrentTask.TaskName);
        }
    }
    /// <summary>
    /// Sort of a listener when current task is changed. Is coroutine started in start to "listen" for change.
    /// Currently only used for debug logging our current task.
    /// </summary>
    private IEnumerator CorOnCurrentTaskChanges()
    {
        while (true)
        {
            yield return new WaitForSeconds(.1f);
            // Runs code once when current task != old task
            if (myCurrentTask != oldTask)
            {
                oldTask = myCurrentTask;
                // Put code under here:...
                Debug.Log(myCurrentTask.TaskName);
            }
        }
    }


    /// <summary>
    /// Gets a default schedule of: -> arriving to work -> work -> lunch -> work -> leave work ->
    /// </summary>
    /// <returns>returns a list of tasks with the default tasks inside</returns>
    private List<Task> GetDefaultScheduleTasks()
    {
        List<Task> tempSchedule = new List<Task>();

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
            tempSchedule.Add(new Task(defaultTask.TaskTime[i], defaultTask.TaskName[i]));
        }

        return tempSchedule;
    }

}
