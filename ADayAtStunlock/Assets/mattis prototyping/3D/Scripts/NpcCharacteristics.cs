using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcCharacteristics : MonoBehaviour {

    [HideInInspector]
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

    EconomyManager manager;
    bool working;


    // Use this for initialization
    void Start () {
        output = Random.Range(1500f, 3500f);
        salary = Random.Range(2500f, 4500f);
        
        working = true;

        motivation = 100;
        motivationDecay = Random.Range(0.5f, 2.5f);
        motivationSlider = transform.GetChild(0).GetChild(1).GetComponent<Slider>();

        happiness = 100;
        happinessDecay = Random.Range(0.5f,2.5f);
        happinessSlider = transform.GetChild(0).GetChild(0).GetComponent<Slider>();

        manager = FindObjectOfType<EconomyManager>();


        
    }
	
	// Update is called once per frame
	void Update () {

        
            motivation = Mathf.Clamp(motivation -= (Time.deltaTime * motivationDecay), 0, 100);
            happiness = Mathf.Clamp(happiness -= (Time.deltaTime * happinessDecay), 0, 100);
        
            

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
