using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRaycast : MonoBehaviour {

    int layerMask;
    public static RaycastHit hit;
    public static Ray ray;
    public static bool mouseStart;

    // Use this for initialization
    void Start ()
    {
        layerMask = LayerMask.GetMask("Floor");
        hit = new RaycastHit();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonUp(0))
        {
            mouseStart = false;
        }
        
        if (Input.GetMouseButton(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f, layerMask);
            
        }
        
        if (hit.transform != null && Input.GetMouseButtonDown(0))
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //print("Mouse start is set to true");
                mouseStart = true;
            }
            else
            {
                //print("Mouse start is set to false");
                mouseStart = false;
            }
        }
    }
}
