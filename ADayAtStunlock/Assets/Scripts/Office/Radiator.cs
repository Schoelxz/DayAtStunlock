using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radiator : MonoBehaviour
{
    /*
     * JÅ:
     * Maybe change this from a collider to checking distance to NPCs instead?
     * Collisions can be a bit clunky. ;)
    */

    List<DAS.NPC> nearbyNpcs = new List<DAS.NPC>();
    bool isBroken;
    Button fixButton;
    Image fixButtonImage;


    AudioManager audioManager;

	void Start () {

        isBroken = false;

        
        fixButton = gameObject.GetComponentInChildren<Button>();
        fixButton.onClick.AddListener(RadiatorEnd);

        fixButtonImage = gameObject.GetComponentInChildren<Image>();
        fixButtonImage.enabled = false;

        audioManager = GameObject.FindObjectOfType<AudioManager>();

    }
	
	void Update () {

        if (isBroken)
        {
            //Make all npcs in the nearbyNpcs list sad. The list gets updated by the triggerbox on the radiator.
            foreach (var npc in nearbyNpcs)
            {
                npc.myFeelings.Happiness -= 0.03f * DAS.TimeSystem.DeltaTime;
            }
        }
	}

    public void RadiatorStart()
    {
        isBroken = true;
        fixButtonImage.enabled = true;

        //Play radiator sound here
        if(audioManager != null)
        {
            print("Trying to play broken radiator sound");
            audioManager.Play("RadiatorBroken");
        }
        
    }

    void RadiatorEnd()
    {   
        fixButtonImage.enabled = false;
        isBroken = false;

        //Pause radiator sound here
        //Play fix radiator sound here
        if (audioManager != null)
        {
            print("Trying to stop broken radiator sound");
            audioManager.Stop("RadiatorBroken");
            audioManager.Play("RadiatorRepair");
        }
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
