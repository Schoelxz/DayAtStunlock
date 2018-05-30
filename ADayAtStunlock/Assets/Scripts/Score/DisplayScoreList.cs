using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScoreList : MonoBehaviour {

    private Text[] highscoreNames;
    private Text[] highscoreScores;
    private Text[] highscoreTimes;

    // Use this for initialization
    void Start()
    {
        //AnimateHighscoreList();

    }
    private void OnEnable()
    {
        AnimateHighscoreList();
    }

    private void OnDisable()
    {
        //ClearDisplayedHighscoreList();
    }

    // Update is called once per frame
    void Update ()
    {
        //DisplayScores();
    }
    
    //Initialize lists
    private void InitializeLists()
    {
        highscoreNames = gameObject.transform.Find("Names").gameObject.GetComponentsInChildren<Text>();
        highscoreScores = gameObject.transform.Find("Scores").gameObject.GetComponentsInChildren<Text>();
        highscoreTimes = gameObject.transform.Find("Times").gameObject.GetComponentsInChildren<Text>();

        foreach (var item in highscoreNames)
        {
            item.text = "-";
        }
        foreach (var item in highscoreScores)
        {
            item.text = "-";
        }
        foreach (var item in highscoreTimes)
        {
            item.text = "-";
        }
    }

    public void DisplayScores()
    {
        for (int i = 0; i < Highscore.scores.Count; i++)
        {
            highscoreNames[i].text = Highscore.scores[i].Name;
            highscoreScores[i].text = Highscore.scores[i].Amount.ToString("n0") + " PTS";
            highscoreTimes[i].text = Highscore.scores[i].Time.ToString("n0") + " s";
        }
    }
    public void AnimateHighscoreList()
    {
        InitializeLists();
        ClearDisplayedHighscoreList();
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        float speed = 0.02f;
        for (int i = 0; i < Highscore.scores.Count; i++)
        {
            if(Highscore.scores[i] == Highscore.latestAddedScore)
            {
                highscoreNames[i].color = Color.yellow;
                highscoreScores[i].color = Color.yellow;
                highscoreTimes[i].color = Color.yellow;
            }
            else
            {
                highscoreNames[i].color = Color.white;
                highscoreScores[i].color = Color.white;
                highscoreTimes[i].color = Color.white;
            }

            foreach (char letter in Highscore.scores[i].Name.ToCharArray())
            {
                highscoreNames[i].text += letter;
                yield return new WaitForSecondsRealtime(speed);
            }
            string score = Highscore.scores[i].Amount.ToString("n0") + " PTS";
            foreach (char letter in score.ToCharArray())
            {
                highscoreScores[i].text += letter;
                yield return new WaitForSecondsRealtime(speed);
            }
            string time = Highscore.scores[i].Time.ToString("n0") + " s";
            foreach (char letter in time.ToCharArray())
            {
                highscoreTimes[i].text += letter;
                yield return new WaitForSecondsRealtime(speed);
            }
        }
    }

    private void ClearDisplayedHighscoreList()
    {

        foreach (Text name in highscoreNames)
        {
            name.text = " ";
        }
        foreach (Text score in highscoreScores)
        {
            score.text = " ";
        }
        foreach (Text time in highscoreTimes)
        {
            time.text = " ";
        }
    }
}
