using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToggler : MonoBehaviour {

    Button[] buttons;
    
    GameObject player;
    int distance;

	// Use this for initialization
	void Start () {

        buttons = GetComponentsInChildren<Button>(true);
        
        player = GameObject.Find("Player");
        distance = 5;

	}
	
	// Update is called once per frame
	void Update () {

        if(buttons != null && player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < distance)
            {
                foreach (var b in buttons)
                {
                    b.gameObject.SetActive(true);
                }
            }
            else
            {
                if (buttons != null)
                {
                    foreach (var b in buttons)
                    {
                        b.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
