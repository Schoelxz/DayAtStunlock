using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelChanger : MonoBehaviour {

    public GameObject alienModel;
    public GameObject personModel;
    private DAS.NPCMovement npcMovement;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ToggleModel();
        }

	}

    public void ToggleModel()
    {
        npcMovement = gameObject.GetComponent<DAS.NPCMovement>();

        if (alienModel.activeInHierarchy)
        {
            alienModel.SetActive(false);
            personModel.SetActive(true);
            npcMovement.ToggleAnimator();
        }
        else
        {
            personModel.SetActive(false);
            alienModel.SetActive(true);
            npcMovement.ToggleAnimator();
        }


    }
}
