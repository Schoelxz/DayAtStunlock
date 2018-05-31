using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathingScale : MonoBehaviour
{
    private float x, y, z;

    private void Start()
    {
        x = gameObject.transform.localScale.x;
        y = gameObject.transform.localScale.y;
        z = gameObject.transform.localScale.z;
    }

    void Update ()
    {
        gameObject.transform.localScale = new Vector3((Mathf.Cos(Time.time*3) / 20) + x, Mathf.Cos(Time.time * 3) / 20 + y, Mathf.Cos(Time.time * 3) / 20 + z);
	}
}
