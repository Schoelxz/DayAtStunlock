using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlTime : MonoBehaviour
{
    public Toggle fastForward;
    private bool isFastForward = false;

	// Use this for initialization
	void Start ()
    {
        fastForward = GetComponent<Toggle>();
        fastForward.onValueChanged.AddListener((value) => { ToggleFastForward(value); }  );
	}

    private void Update()
    {
        if (Time.timeScale == 0)
            fastForward.enabled = false;
        else
            fastForward.enabled = true;

        if (Input.GetKeyDown(KeyCode.LeftControl) && fastForward.enabled)
            fastForward.isOn = !fastForward.isOn;
    }

    private void FixedUpdate()
    {
        // run in update to not conflict with pause (bad, but workable solution).
        if(fastForward.enabled)
            ToggleFastForward();
    }

    void ToggleFastForward()
    {
        if (isFastForward)
        {
            Time.timeScale = 3;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    bool ToggleFastForward(bool value)
    {
        if(!value)
        {
            isFastForward = true;
            Time.timeScale = 3;
            return true;
        }
        else
        {
            isFastForward = false;
            Time.timeScale = 1;
            return false;
        }
    }
	
}
