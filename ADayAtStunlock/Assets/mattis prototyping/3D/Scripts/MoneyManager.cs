﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour {

    NpcIcons[] npcs;

    int working;
    public float salary;
    int output;
    int startMoney;

    //Use this as a display while playing
    static public float currentMoney;

    //Use this for highscore
    static public float moneyEarned;

	// Use this for initialization
	void Start () {
        startMoney = 1000000;
        currentMoney = startMoney;
        working = 0;
        salary = 15000;
        output = 18500;
        npcs = FindObjectsOfType<NpcIcons>();
	}
	
	// Update is called once per frame
	void Update () {

        //Counts how many npcs are working
        foreach (var n in npcs)
        {
            if(n.hasStatus == false)
            {
                working++;
            }
        }

        //Calculates money
        currentMoney += output * working * Time.deltaTime;
        moneyEarned += output * working * Time.deltaTime;
        currentMoney -= salary * npcs.Length *  Time.deltaTime;

        //Increases salary for workers over time
        salary += Time.deltaTime * 10;

        working = 0;
	}
}
