using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreListScreen : MonoBehaviour {

    GameObject namesObject;
    static Text[] names;

    GameObject scoresObject;
    static Text[] scores;

    static GameObject thisInstance;


    private void Awake()
    {
        thisInstance = gameObject;
        namesObject = gameObject.transform.GetChild(0).gameObject;
        names = namesObject.GetComponentsInChildren<Text>(true);

        scoresObject = gameObject.transform.GetChild(1).gameObject;
        scores = scoresObject.GetComponentsInChildren<Text>(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            this.gameObject.SetActive(true);
        }
    }


    static public void DisplayScores()
    {
        for (int i = 0; i < Highscore.scores.Count; i++)
        {
            names[i].text = Highscore.scores[i].Name;
            scores[i].text = Highscore.scores[i].Amount.ToString("n0");
        }
    }

    static public void DisplayScreen()
    {
        thisInstance.SetActive(true);
    }
}
