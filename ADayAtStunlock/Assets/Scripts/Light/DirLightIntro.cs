using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirLightIntro : MonoBehaviour
{

    Light dirLight;

	// Use this for initialization
	void Start ()
    {
        dirLight = GetComponent<Light>();
        dirLight.intensity = 0;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (dirLight.intensity < 1)
            dirLight.intensity += DAS.TimeSystem.DeltaTime / 3;
        else
            Destroy(this);
		
	}
}
