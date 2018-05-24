using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard}

    static public Difficulty currentDifficulty;

    public static DifficultyManager s_myInstance;

    int mediumDifficultyDelay;
    int hardDifficultyDelay;

    public int timeAtNextDifficulty
    {
        get
        {
            if (DAS.TimeSystem.TimePassedSeconds <= mediumDifficultyDelay)
                return mediumDifficultyDelay;
            else if (DAS.TimeSystem.TimePassedSeconds <= hardDifficultyDelay)
                return hardDifficultyDelay;
            else
                return 0;
        }
    }
    public int timeTilNextDifficulty
    {
        get
        {
            if (DAS.TimeSystem.TimePassedSeconds <= mediumDifficultyDelay)
                return mediumDifficultyDelay - (int)DAS.TimeSystem.TimePassedSeconds;
            else if (DAS.TimeSystem.TimePassedSeconds <= hardDifficultyDelay)
                return hardDifficultyDelay - (int)DAS.TimeSystem.TimePassedSeconds;
            else
                return 0;
        }
    }

    private RandomEventTrigger m_randomEvent;

    private void Awake()
    {
        if (s_myInstance == null)
            s_myInstance = this;
        else
            Destroy(this);
    }

    // Use this for initialization
    void Start ()
    {
        mediumDifficultyDelay = 100;
        hardDifficultyDelay = mediumDifficultyDelay + 240;

        currentDifficulty = Difficulty.Easy;

        m_randomEvent = GameObject.FindObjectOfType<RandomEventTrigger>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currentDifficulty == Difficulty.Hard)
            return;

        if (DAS.TimeSystem.TimePassedSeconds > mediumDifficultyDelay && currentDifficulty == Difficulty.Easy)
            if (DAS.NpcCreator.MaxNumberOfNPCsByWorkseatAmount == DAS.NPC.s_npcList.Count) //When all npcs have spawned, we can increase the difficulty and move on
            {
                currentDifficulty = Difficulty.Medium;
                m_randomEvent.WhenDifficultyIncreases();
            }

        if (DAS.TimeSystem.TimePassedSeconds > hardDifficultyDelay && currentDifficulty == Difficulty.Medium)
        {
            currentDifficulty = Difficulty.Hard;
            m_randomEvent.WhenDifficultyIncreases();
        }
    }
}
