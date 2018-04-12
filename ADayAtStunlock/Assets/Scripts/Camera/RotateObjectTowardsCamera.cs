using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectTowardsCamera : MonoBehaviour
{
    public bool reverse, spinZ180, spinY180, spinX180;

    private void Start()
    {
        transform.rotation = Quaternion.identity;
    }
    void Update ()
    {
        if(reverse)
            transform.forward = -Camera.main.transform.forward;
        else
            transform.forward = Camera.main.transform.forward;

        if(spinZ180)
            transform.Rotate(0, 0, 180);
        if (spinY180)
            transform.Rotate(0, 180, 0);
        if (spinX180)
            transform.Rotate(180, 0, 0);
    }
}
