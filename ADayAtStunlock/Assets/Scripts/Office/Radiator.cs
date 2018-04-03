using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radiator : MonoBehaviour {

    List<DAS.NPC> nearbyNpcs = new List<DAS.NPC>();
    bool isBroken;
    Button fixButton;
    Image fixButtonImage;
    
	void Start () {

        isBroken = false;

        fixButton = gameObject.GetComponentInChildren<Button>();
        fixButtonImage = gameObject.GetComponentInChildren<Image>();

        fixButton.onClick.AddListener(RadiatorEnd);
        fixButtonImage.enabled = false;
	}
	
	void Update () {

        if (isBroken)
        {
            //Make all npcs in the nearbyNpcs list sad. The list gets updated by the triggerbox on the radiator.
            foreach (var npc in nearbyNpcs)
            {
                npc.myFeelings.Happiness -= 0.03f * Time.deltaTime;
            }
        }
	}

    public void RadiatorStart()
    {
        isBroken = true;
        fixButtonImage.enabled = true;

        //Play sound effect here
    }

    void RadiatorEnd()
    {   
        fixButtonImage.enabled = false;
        isBroken = false;

        //Pause sound effect here
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
