using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statics : MonoBehaviour {

    static public NpcCharacteristics[] npcs;

    // Use this for initialization
    void Start () {

        npcs = FindObjectsOfType<NpcCharacteristics>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
