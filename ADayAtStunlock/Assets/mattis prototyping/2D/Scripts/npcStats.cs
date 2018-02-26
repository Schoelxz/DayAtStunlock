using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class npcStats : MonoBehaviour {

    [HideInInspector]
    public bool acceptInputs;  //Controls whether this npc can be interacted with by the player
    public float output;
    
    //Motivation stuff
    public float motivation;
    float motivationDecay;
    Slider motivationSlider;

    //Happiness stuff
    public float happiness;
    float happinessDecay;
    Slider happinessSlider;

    [HideInInspector]
    public float salary;
    [HideInInspector]
    public float mood;

    moneyManager manager;
    bool working;
    dayTransition dayManager;


    // Use this for initialization
    void Start () {
        acceptInputs = false;
        output = Random.Range(1500f, 3500f);
        salary = Random.Range(2500f, 4500f);

        dayManager = FindObjectOfType<dayTransition>();
        working = true;

        motivation = 100;
        motivationDecay = Random.Range(0.5f, 2.5f);
        motivationSlider = transform.GetChild(0).GetChild(0).GetComponent<Slider>();

        happiness = 100;
        happinessDecay = Random.Range(0.5f, 2.5f);
        happinessSlider = transform.GetChild(1).GetChild(0).GetComponent<Slider>();

        manager = FindObjectOfType<moneyManager>();


        
    }
	
	// Update is called once per frame
	void Update () {

        if(dayManager.timePassing)
        {
            motivation = Mathf.Clamp(motivation -= (Time.deltaTime * motivationDecay), 0, 100);
            happiness = Mathf.Clamp(happiness -= (Time.deltaTime * happinessDecay), 0, 100);
        
            if (acceptInputs) //acceptInputs status comes from the triggerUI script that detects whether the player is near an enemy,
            {                 // only use this for inputs that should affect the enemies when you're in their vicinity
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    motivation = Mathf.Clamp(motivation += 50, 0, 100);
                    happiness = Mathf.Clamp(happiness += 50, 0, 100);
                }
            }

            happinessSlider.value = happiness;          //Updates the UI sliders with new values each frame
            motivationSlider.value = motivation;

            if(happiness < 10 || motivation < 10)
            {
                if(working)
                {
                    manager.currentlyWorking -= 1;
                    working = false;
                }
                mood = 0;
            
            
            }
            else
            {
                if(!working)
                {
                    working = true;
                    manager.currentlyWorking += 1;
                }
                mood = (((happiness + motivation) / 2) / 100) + 1; //Makes this a nice number to work with, between 1 and 2, used as a multiplier for output. 
            }

        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            acceptInputs = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            acceptInputs = false;
        }
    }
}
