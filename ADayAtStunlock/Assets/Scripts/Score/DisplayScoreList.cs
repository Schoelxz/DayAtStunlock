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
        //DisplayScores();
        AnimateHighscoreList();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //DisplayScores();
    }

    public void DisplayScores()
    {
        for (int i = 0; i < Highscore.scores.Count; i++)
        {
            names[i].text = Highscore.scores[i].Name;
            scores[i].text = Highscore.scores[i].Amount.ToString("n0") + " PTS";
            times[i].text = Highscore.scores[i].Time.ToString("n0") + " s";
        }
    }
    public void AnimateHighscoreList()
    {
        ClearHighscoreList();
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        float speed = 0.05f;
        for (int i = 0; i < Highscore.scores.Count; i++)
        {
            foreach (char letter in Highscore.scores[i].Name.ToCharArray())
            {
                names[i].text += letter;
                yield return new WaitForSeconds(speed);
            }
            string score = Highscore.scores[i].Amount.ToString("n0") + " PTS";
            foreach (char letter in score.ToCharArray())
            {
                scores[i].text += letter;
                yield return new WaitForSeconds(speed);
            }
            string time = Highscore.scores[i].Time.ToString("n0") + " s";
            foreach (char letter in time.ToCharArray())
            {
                times[i].text += letter;
                yield return new WaitForSeconds(speed);
            }
        }
    }

    private void ClearHighscoreList()
    {

        foreach (Text name in names)
        {
            name.text = " ";
        }
        foreach (Text score in scores)
        {
            score.text = " ";
        }
        foreach (Text time in times)
        {
            time.text = " ";
        }
    }
}
