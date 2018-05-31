using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreDisplay : MonoBehaviour
{
    private Text m_scoreText;

    private void Awake()
    {
        m_scoreText = GetComponent<Text>();
    }

    public void SetScore(float newScore, float moneyChangeLastFrame)
    {
        if (m_scoreText == null)
            return;

        m_scoreText.text = newScore.ToString("C000000000");
        if (newScore < moneyChangeLastFrame && m_scoreText.color != Color.red)
        {
            MoneyManager.IsEarningMoney = false;
            m_scoreText.color = Color.red;
        }
        else if (newScore > moneyChangeLastFrame && m_scoreText.color != Color.green)
        {
            MoneyManager.IsEarningMoney = true;
            m_scoreText.color = Color.green;
        }
        else if (newScore == moneyChangeLastFrame && m_scoreText.color != Color.black)
            m_scoreText.color = Color.black;
    }
}
