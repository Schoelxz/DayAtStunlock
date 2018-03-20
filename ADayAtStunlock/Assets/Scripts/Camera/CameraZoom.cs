using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    [SerializeField] private GameObject m_cameraRef;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("mouse Scrollwheel") < 0)
        {

        }
	}
}
