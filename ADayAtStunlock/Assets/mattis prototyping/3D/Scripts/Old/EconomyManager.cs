using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour {

    //Money stuff
    int startMoney;
    float currentMoney;
    Text displayMoney;

    float moneyEarned;
    Text displayEarn;

    float moneySpent;
    Text displaySpend;

    Text displayWorking;
    public int currentlyWorking;
    
    
    //Employee stuff
    Text displayHappiness;
    Text displayMotivation;
    float happinessAverage;
    float motivationAverage;

    //Others
    float timer;

    // Use this for initialization
    void Start () {
        startMoney = 1000000;
        currentMoney = startMoney;

        displayMoney = transform.GetChild(0).GetComponent<Text>();
        displayEarn = transform.GetChild(1).GetComponent<Text>();
        displaySpend = transform.GetChild(2).GetComponent<Text>();
        displayWorking = transform.GetChild(3).GetComponent<Text>();
        displayHappiness = transform.GetChild(4).GetComponent<Text>();
        displayMotivation = transform.GetChild(5).GetComponent<Text>();

        currentlyWorking = Statics.npcs.Length;

        timer = 0;
        happinessAverage = 0;
        motivationAverage = 0;
        
    }
	 
	// Update is called once per frame
	void Update () {

        
        timer = Time.deltaTime;
            for (int i = 0; i < Statics.npcs.Length; i++)
            {
                happinessAverage += Statics.npcs[i].happiness;
                motivationAverage += Statics.npcs[i].motivation;

                currentMoney -= Statics.npcs[i].salary * timer; //Deducts salaries
                moneySpent += Statics.npcs[i].salary * timer;

                currentMoney += Statics.npcs[i].mood * Statics.npcs[i].output * timer; //Adds employee output
                moneyEarned += Statics.npcs[i].mood * Statics.npcs[i].output * timer;

                displayMoney.text = "Cash: $" + System.Math.Round(currentMoney, 2).ToString("n0"); //Displays current money
            }

            //Average Happiness UI
            happinessAverage = happinessAverage / (Statics.npcs.Length +1);
            displayHappiness.text = "Average Happiness = " + System.Math.Round(happinessAverage,2).ToString("n0") + "%";

            //Average Motivation UI
            motivationAverage = motivationAverage / (Statics.npcs.Length + 1);
            displayMotivation.text = "Average Motivation = " + System.Math.Round(motivationAverage,2).ToString("n0") + "%";

            displaySpend.text = "Money Spent: $" + moneySpent.ToString("n0");
            displayEarn.text = "Money Earned: $" + moneyEarned.ToString("n0");

            displayWorking.text = "Employees currently working: " + currentlyWorking.ToString("n0") + "/" + Statics.npcs.Length.ToString("n0");

        
    }
}
