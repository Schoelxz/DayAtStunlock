using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreListScreen : MonoBehaviour {
   
    Text[] names;
    
    Text[] scores;

    Text[] times;

    public static HighscoreListScreen thisInstance;

    InputField inputField;

    Text playerScoreText;

    Button[] buttons;

    GameObject lists;

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
        //Get  highscore list
        lists = gameObject.transform.Find("LeftPanel").transform.Find("HighscoreList (1)").transform.Find("Lists").gameObject;
        names = lists.transform.Find("Names").gameObject.GetComponentsInChildren<Text>(true);

        scores = lists.transform.Find("Scores").gameObject.GetComponentsInChildren<Text>(true);

        times = lists.transform.Find("Times").gameObject.GetComponentsInChildren<Text>(true);

        inputField = gameObject.GetComponentInChildren<InputField>();
        inputField.contentType = InputField.ContentType.Alphanumeric;
        inputField.onEndEdit.AddListener(delegate { SaveName(); });

        playerScoreText = gameObject.transform.Find("RightPanel").transform.Find("PlayerScore").GetComponentInChildren<Text>(true);

        buttons = gameObject.transform.Find("RightPanel").GetComponentsInChildren<Button>(true);

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

        foreach (var item in buttons)
        {
            item.gameObject.SetActive(false);
        }
    }
    

    public void DisplayScores()
    {
        //for (int i = 0; i < Highscore.scores.Count; i++)
        //{
        //    names[i].text = Highscore.scores[i].Name;
        //    scores[i].text = Highscore.scores[i].Amount.ToString("n0");
        //    times[i].text = Highscore.scores[i].Time.ToString("n0");
        //}
    }

    public void DisplayHighscoreScreen()
    {
        thisInstance.gameObject.SetActive(true);
        inputField.readOnly = false;
        inputField.text = "";
        foreach (var item in buttons)
        {
            item.gameObject.SetActive(false);
        }
        playerScoreText.text = MoneyManager.highscorePoints.ToString("n0");
    }

    void SaveName()
    {
        if(inputField.text.Length > 0)
        {
            Highscore.AddHighscore(inputField.text, (int)MoneyManager.highscorePoints, (int)DAS.TimeSystem.TimePassedSeconds);
        }
        else
        {
            Highscore.AddHighscore("Noname", (int)MoneyManager.highscorePoints, (int)DAS.TimeSystem.TimePassedSeconds);
        }
        Highscore.SortHighscore();
        Highscore.SaveHighscore();
        lists.GetComponent<DisplayScoreList>().AnimateHighscoreList();
        inputField.readOnly = true;

        foreach(var item in buttons)
        {
            item.gameObject.SetActive(true);
        }
    }
    
}
