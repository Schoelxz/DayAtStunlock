using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipMover : MonoBehaviour {
    public Vector3 offset;

    void Start()
    {
        offset.x = this.GetComponent<RectTransform>().sizeDelta.x /2;
        offset.y = this.GetComponent<RectTransform>().sizeDelta.y /2 *-1;

    }

    // Update is called once per frame
    void Update ()
    {   
        //top right
        if (Input.mousePosition.y <= Screen.height/2 && Input.mousePosition.x >= Screen.width /2)
        {
            
        } 
        // lower right
        else if (Input.mousePosition.y >= Screen.height / 2 && Input.mousePosition.x >= Screen.width / 2)
        {
            offset.y = offset.y * -1;
        }
        //top left
        else if (Input.mousePosition.y <= Screen.height / 2 && Input.mousePosition.x <= Screen.width / 2)
        {
            offset.x = offset.x * -1;
            
        }
        //lower left
        else if (Input.mousePosition.y >= Screen.height / 2 && Input.mousePosition.x <= Screen.width / 2)
        {
            offset.x = offset.x * -1;
            offset.y = offset.y * -1;
        }
       

     transform.position = Input.mousePosition + offset;
    }
}
