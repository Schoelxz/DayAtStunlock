using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private new Camera camera;
    public static float shakeDuration = 0;
    private float shakeAmount;
    private Vector3 originalPos;

	void Start ()
    {
        camera = Camera.main;
        originalPos = camera.transform.localPosition;
        shakeAmount = 0.2f;
	}

	void Update ()
    {	
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
        }
    }
}
