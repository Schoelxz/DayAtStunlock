using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour {

    float time;
    [SerializeField] float blinkDelay = 0.2f;
    Light light;
    // Use this for initialization
	void Start () {
        light = GetComponent<Light>();
        light.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time > blinkDelay)
        {
            light.enabled = false;
        }
        if(time > blinkDelay * 2)
        {
            time = 0;
            light.enabled = true;
        }
	}
}
