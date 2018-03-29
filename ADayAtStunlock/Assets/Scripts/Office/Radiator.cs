using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radiator : MonoBehaviour {

    List<DAS.NPC> nearbyNpcs = new List<DAS.NPC>();

    bool isBroken;



	// Use this for initialization
	void Start () {

        isBroken = false;

	}
	
	// Update is called once per frame
	void Update () {
        
        if (isBroken)
        {
            //Decay all npcs in the nearbyNpcs list that gets updated by a triggerbox
            foreach (var npc in nearbyNpcs)
            {
                npc.myFeelings.Happiness -= 0.03f * Time.deltaTime;
            }
        }
	}


    public void RadiatorNoise()
    {
        isBroken = true;
        print("Radiator noise started");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            nearbyNpcs.Add(other.GetComponent<DAS.NPC>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            nearbyNpcs.Remove(other.GetComponent<DAS.NPC>());
        }
    }

   
}
