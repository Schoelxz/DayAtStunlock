using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleManager : MonoBehaviour
{
    public static List<Schedule> AllSchedules = new List<Schedule>();
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(DAS.TimeSystem.TimePassedSeconds % 1440);
        //Debug.Log(DAS.TimeSystem.TimePassedMinutes % 24);
    }

    public void AddTaskToSchedule(Schedule schedule, Schedule.Task task)
    {
        schedule.myScheduleTasks.Add(task);
    }
    public void AddTaskToSchedule(Schedule schedule, float startTime, float endTime, string taskName)
    {
        schedule.myScheduleTasks.Add(new Schedule.Task(startTime, endTime, taskName));
    }
    public void AddTaskToSchedule(Schedule schedule, Vector2 timeRange, string taskName)
    {
        schedule.myScheduleTasks.Add(new Schedule.Task(timeRange, taskName));
    }
    
    public void ModifyScheduleTask(Schedule.Task task, float startTime, float endTime, string taskName)
    {
        task.StartTime = startTime;
        task.EndTime = endTime;
        task.TaskName = taskName;
    }
    public void ModifyScheduleTask(Schedule.Task task, Vector2 timeRange, string taskName)
    {
        task.TimeRange = timeRange;
        task.TaskName = taskName;
    }

    public void RemoveScheduleTask(Schedule schedule, Schedule.Task task)
    {
        schedule.myScheduleTasks.Remove(task);
    }

    public static void CheckConflictingSchedule()
    {
        for (int i = 0; i < AllSchedules.Count; i++)
        {
            for (int j = 0; j < AllSchedules[i].myScheduleTasks.Count; j++)
            {
                for (int k = 0; k < AllSchedules[i].myScheduleTasks.Count; k++)
                {
                    List<Schedule.Task> tasks = AllSchedules[i].myScheduleTasks;
                    if (j == k)
                        continue;

                    if (tasks[j].StartTime == tasks[k].StartTime)
                    {
                        Debug.LogAssertion("a tasks' start time is the same as another task.");
                    }

                }
            }
        }
    }


}
