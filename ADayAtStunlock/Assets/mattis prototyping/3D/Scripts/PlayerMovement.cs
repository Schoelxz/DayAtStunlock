﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {
    
    Rigidbody rb;

    [Range(2, 10)]
    public float movementSpeed;
    
   
	
	// Update is called once per frame
	void Update ()
    {
        if(PlayerRaycast.hit.transform != null && !EventSystem.current.IsPointerOverGameObject())
        {
            PlayerAbilities.usingVicinity = false;
            transform.position = Vector3.MoveTowards(transform.position, PlayerRaycast.hit.point, movementSpeed * Time.deltaTime);
        }
         
                

        
    }
}
