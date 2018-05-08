using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Camera myCamera;
    public static float shakeDuration = 0;
    private float shakeAmount;
    private Vector3 originalPos;

	void Start ()
    {
        myCamera = Camera.main;
        originalPos = myCamera.transform.localPosition;
        shakeAmount = 0.2f;
	}

	void Update ()
    {	
        if(Vector3.Distance(new Vector3(0,0,0), myCamera.transform.localPosition) > 10)//Resets camera position if it strays too far from the player
        {
            myCamera.transform.localPosition = originalPos;
        }

        if (shakeDuration > 0)
        {
            myCamera.transform.localPosition = myCamera.transform.localPosition += Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0;
            myCamera.transform.localPosition = originalPos;
        }
    }
}
