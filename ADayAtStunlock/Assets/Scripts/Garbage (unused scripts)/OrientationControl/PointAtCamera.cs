using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtCamera : MonoBehaviour {

    public new Camera camera;
    // Use this for initialization
    void Start () {
        camera = FindObjectOfType<Camera>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        UpdateOrientationTowardsCamera();

    }

    void UpdateOrientationTowardsCamera()
    {
        gameObject.transform.LookAt(camera.transform);
    }
}
