using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScoreList : MonoBehaviour
{
    private Text[] m_highscoreNames, m_highscoreScores, m_highscoreTimes;

    private void Start()
    {
        AnimateHighscoreList();
    }

    private void OnEnable()
    {
        AnimateHighscoreList();
    }

    private void OnDisable()
    {
        ClearDisplayedHighscoreList();
    }
    
    //Initialize lists
    private void InitializeLists()
    {
        m_highscoreNames = gameObject.transform.Find("Names").gameObject.GetComponentsInChildren<Text>();
        m_highscoreScores = gameObject.transform.Find("Scores").gameObject.GetComponentsInChildren<Text>();
        m_highscoreTimes = gameObject.transform.Find("Times").gameObject.GetComponentsInChildren<Text>();
        ClearDisplayedHighscoreList();
    }

    public void DisplayScores()
    {
        for (int i = 0; i < Highscore.s_playerScoresList.Count; i++)
        {
            m_highscoreNames[i].text = Highscore.s_playerScoresList[i].Name;
            m_highscoreScores[i].text = Highscore.s_playerScoresList[i].Amount.ToString("n0") + " PTS";
            m_highscoreTimes[i].text = Highscore.s_playerScoresList[i].Time.ToString("n0") + " s";
        }
    }
    public void AnimateHighscoreList()
    {
        StopAllCoroutines();
        InitializeLists();
        ClearDisplayedHighscoreList();
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        float speed = 0.01f;
        for (int i = 0; i < Highscore.s_playerScoresList.Count; i++)
        {
            //Set colour
            if(Highscore.s_playerScoresList[i] == Highscore.s_latestAddedScore)
            {
                m_highscoreNames[i].color = Color.yellow;
                m_highscoreScores[i].color = Color.yellow;
                m_highscoreTimes[i].color = Color.yellow;
            }
            else
            {
                m_highscoreNames[i].color = Color.white;
                m_highscoreScores[i].color = Color.white;
                m_highscoreTimes[i].color = Color.white;
            }
            //Type text
            foreach (char letter in Highscore.s_playerScoresList[i].Name.ToCharArray())
            {
                m_highscoreNames[i].text += letter;
                yield return new WaitForSecondsRealtime(speed);
            }

            string score = Highscore.s_playerScoresList[i].Amount.ToString("n0") + " PTS";
            foreach (char letter in score.ToCharArray())
            {
                m_highscoreScores[i].text += letter;
                yield return new WaitForSecondsRealtime(speed);
            }

            string time = Highscore.s_playerScoresList[i].Time.ToString("n0") + " s";
            //highscoreTimes[i].text = Highscore.scores[i].Time.ToString("n0");
            foreach (char letter in time.ToCharArray())
            {
                m_highscoreTimes[i].text += letter;
                yield return new WaitForSecondsRealtime(speed);
            }
        }
    }

    private void ClearDisplayedHighscoreList()
    {
        foreach (var item in m_highscoreNames)
            item.text = "";
        foreach (var item in m_highscoreScores)
            item.text = "";
        foreach (var item in m_highscoreTimes)
            item.text = "";
    }
}
