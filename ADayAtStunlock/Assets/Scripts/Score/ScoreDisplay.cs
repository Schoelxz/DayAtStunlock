using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreDisplay : MonoBehaviour {
    private Text m_scoreText;
	// Use this for initialization
	void Start () {
        m_scoreText = GetComponent<Text>();

    }

    public void SetScore(float newScore, float moneyChangeLastFrame)
    {
        m_scoreText.text = newScore.ToString("C000000000");
        if(newScore< moneyChangeLastFrame && m_scoreText.color != Color.red)
        { 
        m_scoreText.color = Color.red;
        }
        else if(newScore> moneyChangeLastFrame && m_scoreText.color != Color.green)
        {
            m_scoreText.color = Color.green;
        }
    }
}
