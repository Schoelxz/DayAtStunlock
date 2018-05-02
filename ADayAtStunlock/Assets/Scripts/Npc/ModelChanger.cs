using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelChanger : MonoBehaviour
{
    private GameObject alienModel;
    private GameObject personModel;
    private DAS.NPCMovement npcMovement;

    [Range(1, 120)]
    public int stayAsAlienDuration = 45;

    public bool isAlien;

	// Use this for initialization
	void Start ()
    {
        isAlien = false;
        alienModel = gameObject.transform.GetChild(1).gameObject;
        personModel = gameObject.transform.GetChild(0).gameObject;
    }

    public void ToggleModel()
    {
        npcMovement = gameObject.GetComponent<DAS.NPCMovement>();
        
        isAlien = true;
        personModel.SetActive(false);
        alienModel.SetActive(true);
        npcMovement.ToggleAnimator();

        Invoke("AlienOff", stayAsAlienDuration);
    }

    private void AlienOff()
    {
        isAlien = false;
        alienModel.SetActive(false);
        personModel.SetActive(true);
        npcMovement.ToggleAnimator();
    }
}
