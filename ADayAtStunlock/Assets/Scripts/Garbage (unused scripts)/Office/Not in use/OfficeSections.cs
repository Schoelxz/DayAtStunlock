using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeSections : MonoBehaviour
{

    [HideInInspector]
    public List<NpcCharacteristics> npcs = new List<NpcCharacteristics>();


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "NPC")
        {
            print("npc added");
            //Get all npcs in the room and add them to a list
            npcs.Add(collision.GetComponent<NpcCharacteristics>());
        }
        if (collision.tag == ("Player"))
        {
            print("player is in room" + this.name);
            PlayerCharacteristics.currentRoom = this;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.tag == ("Player"))
        {
            print("Player moved out of room" + this.name);
            PlayerCharacteristics.currentRoom = null;
        }

        if (collision.tag == ("NPC"))
        {
            print("Npc removed from list");
            npcs.Remove(collision.GetComponent<NpcCharacteristics>());
        }
    }

}


