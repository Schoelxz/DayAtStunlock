using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayTransition : MonoBehaviour {

    npcStats[] npcs;
    float dayTimer;
    public bool timePassing;

	// Use this for initialization
	void Start () {

        timePassing = true;
        dayTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {

        dayTimer += Time.deltaTime;


        //if(dayTimer > 30)
        //{

        //    timePassing = false;
        //    //bring up ui for end of day interaction
        //}

        if(Input.GetKeyDown(KeyCode.Return))
        {
            timePassing = true;
            dayTimer = 0;
        }

	}
}
