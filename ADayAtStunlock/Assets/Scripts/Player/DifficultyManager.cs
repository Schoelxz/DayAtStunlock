using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour {

    public enum Difficulty { Easy, Medium, Hard}

    static public Difficulty currentDifficulty;

    // !!!
    // Make a singleton of the class instead of making this variable public. This should be an inspector variable.
    static public bool difficultyScalingEnabled = true;

    int mediumDifficultyDelay;
    int hardDifficultyDelay;

    DAS.NpcCreator npcCreator;
    RandomEventTrigger randomEvent;

	// Use this for initialization
	void Start ()
    {
        mediumDifficultyDelay = 100;
        hardDifficultyDelay = mediumDifficultyDelay + 240;

        currentDifficulty = Difficulty.Easy;

        npcCreator = GameObject.FindObjectOfType<DAS.NpcCreator>();
        randomEvent = GameObject.FindObjectOfType<RandomEventTrigger>();
	}
	
	// Update is called once per frame
	void Update () {

        if(difficultyScalingEnabled)
        {
            if (DAS.TimeSystem.TimePassedSeconds > mediumDifficultyDelay && currentDifficulty == Difficulty.Easy)
            {
                npcCreator.MaxAllowedNpcs = DAS.NpcCreator.MaxNumberOfNPCsByWorkseatAmount;

                if (DAS.NpcCreator.MaxNumberOfNPCsByWorkseatAmount == DAS.NPC.s_npcList.Count) //When all npcs have spawned, we can increase the difficulty and move on
                {
                    currentDifficulty = Difficulty.Medium;
                    randomEvent.IncreaseDifficulty();
                }
            }

            if (DAS.TimeSystem.TimePassedSeconds > hardDifficultyDelay && currentDifficulty == Difficulty.Medium)
            {
                currentDifficulty = Difficulty.Hard;
                randomEvent.IncreaseDifficulty();
            }
        }
        
    }
}
