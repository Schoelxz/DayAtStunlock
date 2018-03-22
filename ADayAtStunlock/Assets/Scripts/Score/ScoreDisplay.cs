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

    public void SetScore(float newScore)
    {
        m_scoreText.text = newScore.ToString("C000000000");
    }
}
