using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStatus : MonoBehaviour {
    
    NpcIcons[] npcs;
    int npc;

    public int delay;
    public int interval;


	// Use this for initialization
	void Start () {

        delay = 5;
        interval = 10;   

        npcs = FindObjectsOfType<NpcIcons>();
        
        InvokeRepeating("GiveStatus", delay, interval);
    }
	
    
    void GiveStatus()
    {
        npcs[Random.Range(0, npcs.Length)].DisplayIcon();


        //foreach (var n in npcs)
        //{
        //    if(n.hasStatus == false)
        //    {
        //        n.hasStatus = true;
        //        n.DisplayIcon();
        //        break;
        //    }
        //}
    }

}
