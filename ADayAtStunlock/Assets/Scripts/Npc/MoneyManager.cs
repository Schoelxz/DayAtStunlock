using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(ScoreDisplay))]
public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_endScreen;

    private static MoneyManager myInstance;

    private float GeneratedMoney
    {
        get;
        set;
    }
    private float DeductedMoney
    {
        get;
        set;
    }

    public float npcSalary;
    private int m_npcIncome;
    private int m_startMoney;

    private HighscoreListScreen m_highscoreListScreen;
    private static ScoreDisplay s_scoreDisplay;

    private float fakeMoneyGen, fakeMoneyDed;

    private static float potentialMoneyDifference;
    public static float PotentialMoneyDifference
    {
        get
        {
            return potentialMoneyDifference;
        }
    }

    private static float moneyDifferenceLastGenerate;
    public static float MoneyDifferenceLastGenerate
    {
        get
        {
            return moneyDifferenceLastGenerate;
        }
    }

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
    private float timerGoal = 50;
    private bool run;

    public static int AmountOfCurrentlyWorkingNpcs
    {
        get;
        set;
    }

    private void Awake()
    {
        if (myInstance == null)
            myInstance = this;
        else
        {
            Debug.LogError("Copy of moneymanager!, destroying copy!");
            Destroy(this);
        }
    }

    private void Start ()
    {
        m_highscoreListScreen = HighscoreListScreen.s_thisInstance;
        moneyLost = 0;
        highscorePoints = 0;
        m_startMoney = 40000;
        currentMoney = m_startMoney;
        CurrentMoney = m_startMoney;
        run = false;

        GeneratedMoney = 0;
        DeductedMoney = 0;

        s_scoreDisplay = FindObjectOfType<ScoreDisplay>();
        if (s_scoreDisplay != null)
            s_scoreDisplay.SetScore(CurrentMoney, m_moneyChangeLastFrame);
    }

    private void Update ()
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
        if (timer <= timerGoal)
            timer += Time.fixedDeltaTime;

        if (s_scoreDisplay != null)
        {
            s_scoreDisplay.SetScore(CurrentMoney, m_moneyChangeLastFrame);
            m_moneyChangeLastFrame = currentMoney;
        }

        if (timer >= timerGoal)
            DeductSalary();

        if (float.IsNaN(GeneratedMoney + DeductedMoney))
            return;
        CurrentMoney += GeneratedMoney + DeductedMoney;
        moneyDifferenceLastGenerate = GeneratedMoney + DeductedMoney;
        GeneratedMoney = 0;
        DeductedMoney = 0;

        //Counts how many npcs are working
        foreach (var npc in DAS.NPC.s_npcList)
            if (npc.moveRef.IsCurrentlyWorking)
            {
                fakeMoneyGen += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation) / 2;
                highscorePoints += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation) / 2;
            }

        fakeMoneyGen += 1f / 2;
        
        potentialMoneyDifference = fakeMoneyGen + fakeMoneyDed;

        fakeMoneyGen = 0;
        fakeMoneyDed = 0;
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
                MoneyManager.myInstance.GeneratedMoney += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation)/2;
                highscorePoints  += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation)/2;
            }
        }

        MoneyManager.myInstance.GeneratedMoney += 1f/2;
        highscorePoints += 1f/2;
    }
    /// <summary>
    /// Invoked. Removes money, more if NPCs are unhappy.
    /// </summary>
    private void DeductSalary()
    {
        foreach (var npc in DAS.NPC.s_npcList)
        {
            if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Easy)
            {
                DeductedMoney -= (0.7f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
                fakeMoneyDed -= (0.7f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
            }
            else if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Medium)
            {
                DeductedMoney -= (0.9f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
                fakeMoneyDed -= (0.9f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
            }
            else if (DifficultyManager.currentDifficulty == DifficultyManager.Difficulty.Hard)
            {
                DeductedMoney -= (1.2f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
                fakeMoneyDed -= (1.2f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
            }
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
