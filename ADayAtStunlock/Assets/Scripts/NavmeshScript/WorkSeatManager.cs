﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//uncomment this to fill workSeats with all workseats in scene.
[ExecuteInEditMode]
public class WorkSeatManager : MonoBehaviour
{
    public List<GameObject> workSeats = new List<GameObject>();
    // Use this for initialization

    void Start ()
    {
        workSeats.Clear();
        List<GameObject> gameobjectList = new List<GameObject>();
        List<GameObject> npcList = new List<GameObject>();

        gameobjectList.AddRange(FindObjectsOfType<GameObject>());

        foreach (var item in gameobjectList)
        {
            if(item.tag == "WorkSeat")
            {
                workSeats.Add(item);
            }
        }

        foreach (var item in gameobjectList)
        {
            if(item.tag == "NPC")
            {
                npcList.Add(item);
            }
        }

        for (int i = 0; i < npcList.Count; i++)
        {
            npcList[i].GetComponent<DAS.NavMovementSimple>().myWorkSeat = workSeats[i];
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
