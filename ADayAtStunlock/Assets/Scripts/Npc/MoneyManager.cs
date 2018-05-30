using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(ScoreDisplay))]
public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    GameObject endScreen;

    public float npcSalary;
    int npcIncome;
    int startMoney;

    HighscoreListScreen highscoreListScreen;
    ScoreDisplay scoreDisplay;

    //Use this as a display while playing
    static public float currentMoney;

    //Use this for highscore
    static public float highscorePoints;

    static public float moneyLost;

    private static float m_moneyChangeLastFrame;
    
    float timer;
    bool run;
    


	void Start ()
    {

        highscoreListScreen = HighscoreListScreen.thisInstance;
        moneyLost = 0;
        highscorePoints = 0;
        startMoney = 40000;
        currentMoney = startMoney;
        m_moneyChangeLastFrame = startMoney;
        run = false;

        scoreDisplay = FindObjectOfType<ScoreDisplay>();

        //InvokeRepeating("DeductSalary", 1, 0.2f);
        
	}

    void Update ()
    {
        //Earn points for staying alive.
        highscorePoints += 10 * Time.deltaTime;

        if(scoreDisplay != null)
        scoreDisplay.SetScore(currentMoney, m_moneyChangeLastFrame);

        if (currentMoney <= 0 && !run)
        {
            run = true;
            LoseGame();
        }
        
    }
    private void LateUpdate()
    {
        m_moneyChangeLastFrame = currentMoney;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (timer <= 2)
            return;

        DeductSalary();
    }

    /// <summary>
    /// Called by the player whilst working. Generates money based on the amount of motivation all NPCs currently has.
    /// </summary>
    public static void GenerateMoney()
    {
        //Counts how many npcs are working
        foreach (var npc in DAS.NPC.s_npcList)
        {
            if (npc.moveRef.IsCurrentlyWorking)
            {
                currentMoney += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation)/2;
                highscorePoints  += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation)/2;
            }
        }

        currentMoney +=     1f/2;
        highscorePoints  +=     1f/2;
    }
    /// <summary>
    /// Invoked. Removes money, more if NPCs are unhappy.
    /// </summary>
    void DeductSalary()
    {
        foreach (var npc in DAS.NPC.s_npcList)
        {
            if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Easy)
                currentMoney -= (0.7f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage)/4))/2;
            else if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium)
                currentMoney -= (0.9f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
            else if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
                currentMoney -= (1.2f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
            //moneyLost += (0.8f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
        }
    }

    /// <summary>
    /// Called when there is no more money.
    /// </summary>
    void LoseGame()
    {
        if (highscoreListScreen != null)
        {
            DAS.TimeSystem.PauseTime();
            highscoreListScreen.DisplayHighscoreScreen();
            //highscoreListScreen.DisplayScores();
        }
        else
            Debug.LogWarning("LoseGame was called with a null HighscoreList");
    }

    public static bool IsEarningMoney
    {
        get
        {
            if (currentMoney > m_moneyChangeLastFrame)
                return true;
            else
                return false;
        }
    }
}
