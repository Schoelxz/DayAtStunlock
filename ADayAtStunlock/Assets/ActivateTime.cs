using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTime : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

}
