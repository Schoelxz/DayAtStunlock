using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public GameObject[] particleObjects;

    public void PlayEffectAt(Vector3 effectPosition, string effectName)
    {
        for (int i = 0; i < particleObjects.Length; i++)
        {
            if (particleObjects[i].name == effectName)
            {
                Instantiate(particleObjects[i], effectPosition, Quaternion.identity);
            }
        }
    }

    public void PlayEffectAt(Vector3 effectPosition, Vector3 offsetPosition, string effectName)
    {
        for(int i = 0; i < particleObjects.Length; i++)
        {
            if(particleObjects[i].name == effectName)
            {
                Instantiate(particleObjects[i], new Vector3(effectPosition.x + offsetPosition.x, effectPosition.y + offsetPosition.y, effectPosition.z + offsetPosition.z), Quaternion.identity);
            }
        }
    }
}
