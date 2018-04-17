using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipMover : MonoBehaviour {
    public Vector3 offset;

    void Start()
    {
        offset.x = this.GetComponent<RectTransform>().sizeDelta.x;
        offset.y = this.GetComponent<RectTransform>().sizeDelta.y /2 *-1;

    }

    // Update is called once per frame
    void Update ()
    {
        transform.position = Input.mousePosition + offset;
	}
}
