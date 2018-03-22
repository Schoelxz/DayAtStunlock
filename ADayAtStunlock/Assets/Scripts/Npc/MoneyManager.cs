﻿using System.Collections;
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

    ScoreDisplay MoneyDisplay;

    //Use this as a display while playing
    static public float currentMoney;

    //Use this for highscore
    static public float moneyEarned;

	void Start ()
    {
        startMoney = 0;
        currentMoney = startMoney;

        MoneyDisplay = FindObjectOfType<ScoreDisplay>();

        InvokeRepeating("DeductSalary", 1, 0.2f);
	}

    void Update ()
    {
        MoneyDisplay.SetScore(currentMoney);
    }

    public static void GenerateMoney()
    {
        //Counts how many npcs are working
        foreach (var npc in DAS.NPC.s_npcList)
        {
            if(npc.moveRef.IsCurrentlyWorking)
                currentMoney += (1 / DAS.NPC.s_npcList.Count) + npc.myFeelings.Motivation;
        }
    }

    void DeductSalary()
    {
        print("Deducting salary");
        foreach (var npc in DAS.NPC.s_npcList)
        {
            currentMoney -= 3f - (npc.myFeelings.Happiness + DAS.NPC.s_happyAverage);
        }
    }

}
