using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game
{
    public static Game current;

    [System.Serializable]
    public struct TimeSystemData
    {
        public float timeMultiplier;
        public float timePassedSeconds;
    }

    public TimeSystemData timeSystemData;


    public Game()
    {
        TimeSystemDefault();
        Debug.Log("Game Constructor Called");
    }


    private void TimeSystemDefault()
    {
        timeSystemData.timeMultiplier = 1;
        timeSystemData.timePassedSeconds = 0;
    }
}
