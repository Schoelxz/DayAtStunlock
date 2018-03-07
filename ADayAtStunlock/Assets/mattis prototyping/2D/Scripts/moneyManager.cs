using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moneyManager : MonoBehaviour {

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

    dayTransition dayManager;

    
    
    //Employee stuff
    npcStats[] employees;
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
        displayMoney = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        displayEarn = transform.GetChild(0).GetChild(3).GetComponent<Text>();
        displaySpend = transform.GetChild(0).GetChild(4).GetComponent<Text>();
        displayWorking = transform.GetChild(0).GetChild(5).GetComponent<Text>();

        employees = FindObjectsOfType<npcStats>();

        currentlyWorking = employees.Length;

        timer = 0;

        dayManager = FindObjectOfType<dayTransition>();

        happinessAverage = 0;
        displayHappiness = transform.GetChild(0).GetChild(1).GetComponent<Text>();

        motivationAverage = 0;
        displayMotivation = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        
    }
	 
	// Update is called once per frame
	void FixedUpdate () {

        
        timer = Time.fixedDeltaTime;
        if(dayManager.timePassing)
        {

        


            for (int i = 0; i < employees.Length; i++)
            {
                happinessAverage += employees[i].happiness;
                motivationAverage += employees[i].motivation;

                currentMoney -= employees[i].salary * timer; //Deducts salaries
                moneySpent += employees[i].salary * timer;

                currentMoney += employees[i].mood * employees[i].output * timer; //Adds employee output
                moneyEarned += employees[i].mood * employees[i].output * timer;

                displayMoney.text = "Cash: $" + System.Math.Round(currentMoney, 2).ToString("n0"); //Displays current money
            }

            //Average Happiness UI
            happinessAverage = happinessAverage / (employees.Length +1);
            displayHappiness.text = "Average Happiness = " + System.Math.Round(happinessAverage,2).ToString("n0") + "%";

            //Average Motivation UI
            motivationAverage = motivationAverage / (employees.Length + 1);
            displayMotivation.text = "Average Motivation = " + System.Math.Round(motivationAverage,2).ToString("n0") + "%";

            displaySpend.text = "Money Spent: $" + moneySpent.ToString("n0");
            displayEarn.text = "Money Earned: $" + moneyEarned.ToString("n0");

            displayWorking.text = "Employees currently working: " + currentlyWorking.ToString("n0") + "/" + employees.Length.ToString("n0");

        }
    }
}
