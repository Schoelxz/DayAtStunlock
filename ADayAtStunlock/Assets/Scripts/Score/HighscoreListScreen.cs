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

    static InputField inputField;

    static Text playerScoreText;

    private void Awake()
    {
        thisInstance = gameObject;

        namesObject = gameObject.transform.GetChild(0).gameObject;
        names = namesObject.GetComponentsInChildren<Text>(true);

        scoresObject = gameObject.transform.GetChild(1).gameObject;
        scores = scoresObject.GetComponentsInChildren<Text>(true);

        inputField = gameObject.GetComponentInChildren<InputField>();
        inputField.contentType = InputField.ContentType.Alphanumeric;
        inputField.onEndEdit.AddListener(delegate { SaveName(); });

        playerScoreText = gameObject.transform.GetChild(4).gameObject.GetComponentInChildren<Text>();

        
    }

    private void Update()
    {
    }


    static public void DisplayScores()
    {
        for (int i = 0; i < Highscore.scores.Count; i++)
        {
            names[i].text = Highscore.scores[i].Name;
            scores[i].text = Highscore.scores[i].Amount.ToString("n0");
        }
    }

    static public void DisplayHighscoreScreen()
    {
        thisInstance.SetActive(true);
        playerScoreText.text = MoneyManager.moneyEarned.ToString("n0");
    }

    static void SaveName()
    {
        if(inputField.text.Length > 0)
        {
            Highscore.AddHighscore(inputField.text, (int)MoneyManager.moneyEarned);
        }
        else
        {
            Highscore.AddHighscore("Noname", (int)MoneyManager.moneyEarned);
        }
        Highscore.SortHighscore();
        Highscore.SaveHighscore();
        DisplayScores();
        inputField.readOnly = true;
    }
    
}
