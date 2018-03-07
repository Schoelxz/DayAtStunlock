using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats : MonoBehaviour {

    [HideInInspector]
    public roomScript currentRoom;
    dayTransition dayManager;



    private void Start()
    {
        dayManager = FindObjectOfType<dayTransition>();
    }

    // Update is called once per frame
    void Update () {
		

        if(dayManager.timePassing)
        {
            
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                if(currentRoom != null)
                {
                    foreach(npcStats n in currentRoom.npcs)
                    {
                        print("Trying to boost mood of npcs in this room");
                        n.happiness += 10;
                        n.motivation += 10;
                    }
                }
            }

        }


    }
}
