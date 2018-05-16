using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreListScreen : MonoBehaviour {
   
    Text[] names;
    
    Text[] scores;

    public static HighscoreListScreen thisInstance;

    InputField inputField;

    Text playerScoreText;

    Button[] buttons;

    private void Awake()
    {

        if(thisInstance == null)
        {
            thisInstance = this;
        }
        else if(thisInstance != this)
        {
            Destroy(gameObject);
        }

        names = gameObject.transform.Find("LeftPanel").transform.Find("Names").gameObject.GetComponentsInChildren<Text>(true);

        scores = gameObject.transform.Find("LeftPanel").transform.Find("Scores").gameObject.GetComponentsInChildren<Text>(true);

        inputField = gameObject.GetComponentInChildren<InputField>();
        inputField.contentType = InputField.ContentType.Alphanumeric;
        inputField.onEndEdit.AddListener(delegate { SaveName(); });

        playerScoreText = gameObject.transform.Find("RightPanel").transform.Find("PlayerScore").GetComponentInChildren<Text>(true);

        buttons = gameObject.transform.Find("RightPanel").GetComponentsInChildren<Button>(true);

        foreach (var item in names)
        {
            item.text = "";
        }

        foreach (var item in scores)
        {
            item.text = "";
        }

        foreach(var item in buttons)
        {
            item.gameObject.SetActive(false);
        }
    }
    

    public void DisplayScores()
    {
        for (int i = 0; i < Highscore.scores.Count; i++)
        {
            names[i].text = Highscore.scores[i].Name;
            scores[i].text = Highscore.scores[i].Amount.ToString("n0");
        }
    }

    public void DisplayHighscoreScreen()
    {
        thisInstance.gameObject.SetActive(true);
        inputField.readOnly = false;
        inputField.text = "";
        playerScoreText.text = MoneyManager.moneyEarned.ToString("n0");
        foreach (var item in buttons)
        {
            item.gameObject.SetActive(false);
        }
    }

    void SaveName()
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

        foreach(var item in buttons)
        {
            item.gameObject.SetActive(true);
        }
    }
    
}
