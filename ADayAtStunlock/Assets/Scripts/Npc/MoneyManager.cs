using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(ScoreDisplay))]
public class MoneyManager : MonoBehaviour
{
    [SerializeField]
    GameObject endScreen;

    public float npcSalary;
    int npcIncome;
    int startMoney;

    ScoreDisplay scoreDisplay;

    //Use this as a display while playing
    static public float currentMoney;

    //Use this for highscore
    static public float moneyEarned;

    static public float moneyLost;

    private float m_moneyChangeLastFrame;
    
    float timer;
    bool run;


	void Start ()
    {
        moneyLost = 0;
        moneyEarned = 0;
        startMoney = 12500;
        currentMoney = startMoney;
        m_moneyChangeLastFrame = startMoney;
        run = false;

        scoreDisplay = FindObjectOfType<ScoreDisplay>();

        //InvokeRepeating("DeductSalary", 1, 0.2f);
	}

    void Update ()
    {
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
                moneyEarned  += (DAS.NPC.s_motivationAverage + npc.myFeelings.Motivation)/2;
            }
        }

        currentMoney +=     1f/2;
        moneyEarned  +=     1f/2;
    }
    /// <summary>
    /// Invoked. Removes money, more if NPCs are unhappy.
    /// </summary>
    void DeductSalary()
    {
        foreach (var npc in DAS.NPC.s_npcList)
        {
            currentMoney -= (0.8f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage)/4))/2;
            //moneyLost += (0.8f - ((npc.myFeelings.Happiness + DAS.NPC.s_happyAverage) / 4)) / 2;
        }
    }

    /// <summary>
    /// Called when there is no more money.
    /// </summary>
    void LoseGame()
    {
        
        Highscore.AddHighscore("Name", (int)moneyEarned);
        Highscore.SortHighscore();
        Highscore.SaveHighscore();
        EndGameCanvas.GameOver();
    }

}
