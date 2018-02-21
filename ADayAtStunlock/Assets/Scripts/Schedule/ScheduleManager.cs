using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleManager : MonoBehaviour
{

    private List<Schedule> AllSchedules = new List<Schedule>();
	
	// Update is called once per frame
	void Update ()
    {
        //DAS.TimeSystem.TimePassedSeconds % 1440;
        //DAS.TimeSystem.TimePassedMinutes % 24;
    }

    public void AddScheduleTask(Schedule schedule, Schedule.Task task)
    {
        schedule.myTasks.Add(task);
    }
    public void AddScheduleTask(Schedule schedule, float startTime, float endTime, string taskName)
    {
        schedule.myTasks.Add(new Schedule.Task(startTime, endTime, taskName));
    }

    public void ModifyScheduleTask(Schedule.Task task)
    {
        
    }

    public void RemoveScheduleTask(Schedule schedule, Schedule.Task task)
    {
        schedule.myTasks.Remove(task);
    }

    public void GetSchedule()
    {

    }

    private void CheckConflictingSchedule()
    {
        
    }


}
