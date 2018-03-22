﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {
    
    float cooldown;
    float timer;

    //Objects that are to be interacted with needs a collider, the "InteractableObject" tag, a proper name and a function for whatever purpose it should serve. See below.

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        
        if (PlayerRaycast.hit.transform != null && Input.GetMouseButtonDown(0))
        {
            if(PlayerRaycast.hit.transform.tag == "InteractableObject" )
            {
                if (PlayerRaycast.hit.transform.name == "CoffeeMaker")
                {
                    CoffeeMaker();
                }

                if (PlayerRaycast.hit.transform.name == "Fridge")
                {
                    
                    Fridge();
                }
            }
        }

    }

    void CoffeeMaker()
    {
        print("Coffee Maker was clicked, Coffee maker function running.");
    }

    void Fridge()
    {
        print("Fridge was clicked, Fridge function running.");
    }
}
