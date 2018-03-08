using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleManager : MonoBehaviour
{
    public static List<Schedule> AllSchedules = new List<Schedule>();

    #region Functions

    /// <summary>
    /// Adds a task to the target schedule.
    /// </summary>
    /// <param name="schedule">target schedule</param>
    /// <param name="task">task to add</param>
    public static void AddTaskToSchedule(Schedule schedule, Schedule.Task task)
    {
        schedule.myScheduleTasks.Add(task);
    }
    /// <summary>
    /// Adds a task to the target schedule.
    /// </summary>
    /// <param name="schedule">target schedule</param>
    /// <param name="startTime">task start time</param>
    /// <param name="endTime">task end time</param>
    /// <param name="taskName">task name</param>
    public static void AddTaskToSchedule(Schedule schedule, float startTime, float endTime, string taskName)
    {
        schedule.myScheduleTasks.Add(new Schedule.Task(startTime, endTime, taskName));
    }
    /// <summary>
    /// Adds a task to the target schedule.
    /// </summary>
    /// <param name="schedule">target schedule</param>
    /// <param name="timeRange">task time range</param>
    /// <param name="taskName">task name</param>
    public static void AddTaskToSchedule(Schedule schedule, Vector2 timeRange, string taskName)
    {
        schedule.myScheduleTasks.Add(new Schedule.Task(timeRange, taskName));
    }

    /// <summary>
    /// Modifies a task inside of a schedule.
    /// </summary>
    /// <param name="task">task to modify</param>
    /// <param name="startTime">task start time</param>
    /// <param name="endTime">task end time</param>
    /// <param name="taskName">task name</param>
    public static void ModifyScheduleTask(Schedule.Task task, float startTime, float endTime, string taskName)
    {
        task.StartTime = startTime;
        task.EndTime = endTime;
        task.TaskName = taskName;
    }
    /// <summary>
    /// Modifies a task inside of a schedule.
    /// </summary>
    /// <param name="task">task to modify</param>
    /// <param name="timeRange">tasks' time range to modify</param>
    /// <param name="taskName">task name</param>
    public static void ModifyScheduleTask(Schedule.Task task, Vector2 timeRange, string taskName)
    {
        task.TimeRange = timeRange;
        task.TaskName = taskName;
    }

    /// <summary>
    /// Removes target task from target schedule
    /// </summary>
    /// <param name="schedule">target schedule</param>
    /// <param name="task">target task</param>
    public static void RemoveScheduleTask(Schedule schedule, Schedule.Task task)
    {
        schedule.myScheduleTasks.Remove(task);
    }

    /// <summary>
    /// Checks all schedules for conflicts and invalidity.
    /// </summary>
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
                    // Check for tasks with same start
                    if (tasks[j].StartTime == tasks[k].StartTime)
                    {
                        Debug.LogAssertion("a tasks' start time is the same as another task.");
                    }
                    // Check for tasks with same end
                    if (tasks[j].EndTime == tasks[k].EndTime)
                    {
                        Debug.LogAssertion("a tasks' end time is the same as another task.");
                    }
                    // Check for tasks within eachothers time range.
                    if (tasks[j].StartTime < tasks[k].EndTime && tasks[k].StartTime < tasks[j].EndTime)
                    {
                        Debug.LogAssertion("a tasks' time range is overlapping another tasks' time range.");
                    }

                }
            }
        }
    }

    #endregion Functions

}
