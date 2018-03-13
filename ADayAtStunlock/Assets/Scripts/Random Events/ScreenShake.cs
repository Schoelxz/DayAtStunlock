using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    Camera camera;
    public static float shakeDuration;
    float shakeAmount;
    Vector3 originalPos;


	// Use this for initialization
	void Start () {
        camera = Camera.main;
        originalPos = camera.transform.localPosition;
        shakeDuration = 0;
        shakeAmount = 1;
   
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Vector3.Distance(new Vector3(0,0,0), camera.transform.localPosition) > 10)//Resets camera position if it strays too far from the player
        {
            camera.transform.localPosition = originalPos;
            }

        if (shakeDuration > 0)
        {
            camera.transform.localPosition = camera.transform.localPosition += Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0;
            camera.transform.localPosition = originalPos;
            RandomEventTrigger.stopWork = false;
        }



    }
}
