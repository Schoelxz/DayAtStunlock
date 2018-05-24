using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScoreList : MonoBehaviour {

    Text[] names;
    Text[] scores;
    Text[] times;

	// Use this for initialization
	void Start ()
    {
        names = gameObject.transform.Find("Names").gameObject.GetComponentsInChildren<Text>();
        scores = gameObject.transform.Find("Scores").gameObject.GetComponentsInChildren<Text>();
        times = gameObject.transform.Find("Times").gameObject.GetComponentsInChildren<Text>();

        foreach (var item in names)
        {
            item.text = "-";
        }
        foreach (var item in scores)
        {
            item.text = "-";
        }
        foreach (var item in times)
        {
            item.text = "-";
        }
        DisplayScores();
	}
	
	// Update is called once per frame
	void Update ()
    {
        DisplayScores();
    }

    public void DisplayScores()
    {
        for (int i = 0; i < Highscore.scores.Count; i++)
        {
            names[i].text = Highscore.scores[i].Name;
            scores[i].text = Highscore.scores[i].Amount.ToString("n0");
            times[i].text = Highscore.scores[i].Time.ToString("n0");
        }
    }
}
