using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {
    
    float cooldown;
    float timer;

	// Use this for initialization
	void Start () {
        cooldown = 0.2f;
        timer = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        timer += Time.deltaTime;

        if (PlayerRaycast.hit.transform != null && Input.GetMouseButtonDown(0))
        {
            print(PlayerRaycast.hit.transform.name);
            if(PlayerRaycast.hit.transform.tag == "InteractableObject" )
            {
                if (PlayerRaycast.hit.transform.name == "CoffeeMaker" && timer > cooldown)
                {
                    timer = 0;
                    CoffeeMaker();
                }

                if (PlayerRaycast.hit.transform.name == "Fridge" && timer > cooldown)
                {
                    timer = 0;
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
