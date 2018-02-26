using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomScript : MonoBehaviour
{

    public List<npcStats> npcs = new List<npcStats>();
    
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NPC")
        {
            print("npc added");
            //Get all npcs in the room and add them to a list
            npcs.Add(collision.GetComponent<npcStats>());
        }
        if (collision.tag == ("Player"))
        {
            print("player added");
            collision.GetComponent<playerStats>().currentRoom = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == ("Player"))
        { 
            collision.GetComponent<playerStats>().currentRoom = null;
        }

        if (collision.tag == ("NPC"))
        {
            npcs.Remove(collision.GetComponent<npcStats>());
        }
    }

}


