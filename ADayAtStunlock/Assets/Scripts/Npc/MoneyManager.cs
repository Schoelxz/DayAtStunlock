using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(ScoreDisplay))]
public class MoneyManager : MonoBehaviour
{
    int m_interval = 5;
    int m_delay = 1;

    [SerializeField]
    GameObject endScreen;

    public float npcSalary;
    int npcIncome;
    int startMoney;

    ScoreDisplay MoneyDisplay;

    //Use this as a display while playing
    static public float currentMoney;

    //Use this for highscore
    static public float moneyEarned;

	void Start ()
    {
        startMoney = 0;
        currentMoney = startMoney;

        //HACK: Needs to be controlled by game time, help
        InvokeRepeating("GenerateMoney", m_delay, m_interval);
        MoneyDisplay = FindObjectOfType<ScoreDisplay>();

        InvokeRepeating("GenerateMoney", 1, 5);
        InvokeRepeating("DeductSalary", 31, 5);
	}

    void Update ()
    {
        MoneyDisplay.SetScore(currentMoney);
    }

    void GenerateMoney()
    {
        //Counts how many npcs are working
        foreach (var npc in DAS.NPC.s_npcList)
        {
            if (npc.moveRef.IsCurrentlyWorking)
            {
                npc.GenerateMoney();
            }
        }
    }

    void DeductSalary()
    {
        print("Deducting salary");
        foreach (var npc in DAS.NPC.s_npcList)
        {
            currentMoney -= 2 - (npc.myFeelings.Happiness + npc.myFeelings.Motivation) + DAS.TimeSystem.TimePassedSeconds / 120;
        }
        
    }

}
