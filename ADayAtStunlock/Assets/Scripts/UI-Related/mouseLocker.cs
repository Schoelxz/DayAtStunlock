using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLocker : MonoBehaviour {

	// this locks the cursor in the screen
	void Update ()
    {
        Cursor.lockState = CursorLockMode.Confined;
		
	}
	
}
