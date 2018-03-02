using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStatus : MonoBehaviour {
    
    NpcIcons[] npcs;

    public int delay;
    public int interval;

	// Use this for initialization
	void Start () {

        delay = 5;
        interval = 5;   

        npcs = FindObjectsOfType<NpcIcons>();
        
        InvokeRepeating("GiveStatus", delay, interval);
    }
	
    
    void GiveStatus()
    {
        
        foreach (var n in npcs)
        {
            if(n.hasStatus == false)
            {
                
                n.hasStatus = true;
                n.DisplayIcon();
                break;
            }
        }
    }

}
