using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour {

    public enum Difficulty { Easy, Medium, Hard}

    static public Difficulty currentDifficulty;
    static public bool difficultyScalingEnabled;

	// Use this for initialization
	void Start () {

        currentDifficulty = Difficulty.Easy;
        difficultyScalingEnabled = true;
   
	}
	
	// Update is called once per frame
	void Update () {

		if(DAS.TimeSystem.TimePassedMinutes > 3 && currentDifficulty != Difficulty.Medium)
        {
            currentDifficulty = Difficulty.Medium;
            print("difficulty is now set to medium");
        }

        if (DAS.TimeSystem.TimePassedMinutes > 6 && currentDifficulty != Difficulty.Hard)
        {
            currentDifficulty = Difficulty.Hard;
            print("difficulty is now set to hard");
        }
    }
}
