using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour {

    [HideInInspector]
    static public OfficeSections currentRoom;
    static public List<NpcCharacteristics> vicinityNPCs = new List<NpcCharacteristics>();



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == ("NPC"))
        {
            vicinityNPCs.Add(other.GetComponent<NpcCharacteristics>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == ("NPC"))
        {
            vicinityNPCs.Remove(other.GetComponent<NpcCharacteristics>());
        }
    }
}
