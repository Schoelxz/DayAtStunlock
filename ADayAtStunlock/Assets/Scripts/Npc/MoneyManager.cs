using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(ScoreDisplay))]
public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_endScreen;

    public float npcSalary;
    private int m_npcIncome;
    private int m_startMoney;

    private HighscoreListScreen m_highscoreListScreen;
    private static ScoreDisplay s_scoreDisplay;

    //Use this as a display while playing
    private static float currentMoney;
    public static float CurrentMoney
    {
        get
        {
            return currentMoney;
        }
        set
        {
            m_moneyChangeLastFrame = currentMoney;
            currentMoney = value;
        }
    }

    //Use this for highscore
    public static float highscorePoints;

    public static float moneyLost;

    private static float m_moneyChangeLastFrame;
    
    private float timer;
    private bool run;

    public int AmountOfCurrentlyWorkingNpcs
    {
        get;
        set;
    }

	void Start ()
    {
        m_highscoreListScreen = HighscoreListScreen.s_thisInstance;
        moneyLost = 0;
        highscorePoints = 0;
        m_startMoney = 40000;
        currentMoney = m_startMoney;
        CurrentMoney = m_startMoney;
        run = false;

        s_scoreDisplay = FindObjectOfType<ScoreDisplay>();
        if (s_scoreDisplay != null)
            s_scoreDisplay.SetScore(CurrentMoney, m_moneyChangeLastFrame);
    }

    void Update ()
    {
        //Earn points for staying alive.
        highscorePoints += 10 * Time.deltaTime;

        if (CurrentMoney <= 0 && !run)
        {
            run = true;
            LoseGame();
        }
        int currentlyWorking = 0;
        foreach (var npc in DAS.NPC.s_npcList)
        {

            if (npc.moveRef.IsCurrentlyWorking)
                currentlyWorking++;
        }
        AmountOfCurrentlyWorkingNpcs = currentlyWorking;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (s_scoreDisplay != null)
        {
            s_scoreDisplay.SetScore(CurrentMoney, m_moneyChangeLastFrame);
            m_moneyChangeLastFrame = currentMoney;
        }

        if (timer <= 50)
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
                CurrentMoney += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation)/2;
                highscorePoints  += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation)/2;
            }
        }

        CurrentMoney    += 1f/2;
        highscorePoints += 1f/2;
    }
    /// <summary>
    /// Invoked. Removes money, more if NPCs are unhappy.
    /// </summary>
    private void DeductSalary()
    {
        foreach (var npc in DAS.NPC.s_npcList)
        {
            if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Easy)
                CurrentMoney -= (0.7f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage)/4))/2;
            else if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium)
                CurrentMoney -= (0.9f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
            else if(DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
                CurrentMoney -= (1.2f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
            //moneyLost += (0.8f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
        }
    }

    /// <summary>
    /// Called when there is no more money.
    /// </summary>
    private void LoseGame()
    {
        if (m_highscoreListScreen != null)
        {
            DAS.TimeSystem.PauseTime();
            m_highscoreListScreen.DisplayHighscoreScreen();
            //highscoreListScreen.DisplayScores();
        }
        else
            Debug.LogWarning("LoseGame was called with a null HighscoreList");
    }

    private static bool isEarningMoney = false;
    public static bool IsEarningMoney
    {
        get
        {
            return isEarningMoney;
        }
        set
        {
            isEarningMoney = value;
        }
    }
}
