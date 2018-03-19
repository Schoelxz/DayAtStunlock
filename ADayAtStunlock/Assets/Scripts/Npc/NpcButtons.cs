using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcButtons : MonoBehaviour {

    Button[] buttons;

    Button happiness;
    Button motivation;
    
                
	// Use this for initialization
	void Start () {
        buttons = GetComponentsInChildren<Button>(true);

        foreach (var b in buttons)
        {
            if(b.name == "Happiness")
            {
                happiness = b;
            }

            if(b.name == "Motivation")
            {
                motivation = b;
            }
        }

        if(motivation != null && happiness != null)
        {
            motivation.onClick.AddListener(AddMotivation);
            happiness.onClick.AddListener(AddHappiness);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddMotivation()
    {
        print("Adds motivation to npc. (Actually doesnt, fix this)");
    }

    void AddHappiness()
    {
        print("Adds happiness to npc. (Actually doesnt, fix this)");
    }
}
