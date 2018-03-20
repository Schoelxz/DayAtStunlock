using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour {

    //NPC[] npcs;

    [SerializeField]
    GameObject endScreen;

    public float npcSalary;
    int npcIncome;
    int startMoney;

    //Use this as a display while playing
    static public float currentMoney;

    //Use this for highscore
    static public float moneyEarned;

	void Start ()
    {
        startMoney = 0;
        currentMoney = startMoney;
        //npcSalary = 15000;
        //npcIncome = 18500;
        //npcs = FindObjectsOfType<NPC>();

        InvokeRepeating("GenerateMoney", 1, 5);
        InvokeRepeating("DeductSalary", 30, 5);
	}

    private void OnGUI()
    {
        GUI.Box(new Rect(300, 0, 100, 50), currentMoney.ToString());
    }

    void Update ()
    {

        //Calculates money
        //currentMoney += npcIncome * Time.deltaTime;
        //moneyEarned += npcIncome * Time.deltaTime;
        //currentMoney -= npcSalary * npcs.Length *  Time.deltaTime;

        //Increases salary for workers over time
        //npcSalary += Time.deltaTime * 10;

        /*if(currentMoney < 0)
        {
            if(!endScreen.activeInHierarchy)
            {
                endScreen.SetActive(true);
                endScreen.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = moneyEarned.ToString("n0");
                moneyEarned = 0;
            }
        }*/
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
