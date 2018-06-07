using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

    List<GameObject> pauseMenus = new List<GameObject>();
    bool shouldBePaused;


	// Use this for initialization
	void Awake () {

        pauseMenus.Add(GameObject.Find("IngameMenu"));
        pauseMenus.Add(GameObject.Find("HighscoreCanvas"));
        pauseMenus.Add(GameObject.Find("TutorialCanvas"));

    }
	
	// Update is called once per frame
	void Update () {
        shouldBePaused = false;
        foreach (var item in pauseMenus)
        {
            if(item.gameObject.activeInHierarchy)
            {
                shouldBePaused = true;
            }
        }

        if(shouldBePaused)
        {
            DAS.TimeSystem.PauseTime();
        }
        else
        {
            DAS.TimeSystem.ResumeTime();
        }
	}
}
