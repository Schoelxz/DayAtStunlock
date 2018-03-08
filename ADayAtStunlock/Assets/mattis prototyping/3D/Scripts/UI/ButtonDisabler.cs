using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour {

    GameObject player;
    Button button;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        button = transform.GetChild(0).GetComponentInChildren<Button>();
	}
	
	// Update is called once per frame
	void Update () {

		if(Vector3.Distance(transform.position, player.transform.position) > 10)
        {
            button.enabled = false;
        }
        else
        {
            button.enabled = true;
        }
	}
}
