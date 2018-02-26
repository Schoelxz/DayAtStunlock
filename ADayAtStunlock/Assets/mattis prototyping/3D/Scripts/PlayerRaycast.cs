using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour {

    int layerMask;
    public static RaycastHit hit;
    public static Ray ray;

    // Use this for initialization
    void Start () {
        layerMask = LayerMask.GetMask("UI", "Floor");
	}
	
	// Update is called once per frame
	void Update ()
    {
        hit = new RaycastHit();
        
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f, layerMask);
        }
    }
}
