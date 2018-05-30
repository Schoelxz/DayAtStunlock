using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreListScreen : MonoBehaviour {
   
    private Text[] names, scores, times;

    public static HighscoreListScreen thisInstance;

    private InputField inputField;

    private Text playerScoreText;

    private Button[] buttons;

    private GameObject lists;

    private void Awake()
    {
        if(thisInstance == null)
            thisInstance = this;
        else
            Destroy(gameObject);

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
            item.text = "-";

        foreach (var item in scores)
            item.text = "-";

        foreach (var item in times)
            item.text = "-";

        foreach (var item in buttons)
            item.gameObject.SetActive(false);
    }

    public void DisplayHighscoreScreen()
    {
        thisInstance.gameObject.SetActive(true);
        inputField.readOnly = false;
        inputField.text = "";

        foreach (var item in buttons)
            item.gameObject.SetActive(false);

        playerScoreText.text = MoneyManager.highscorePoints.ToString("n0");
    }

    private void SaveName()
    {
        if(inputField.text.Length > 0)
            Highscore.AddHighscore(inputField.text, (int)MoneyManager.highscorePoints, (int)DAS.TimeSystem.TimePassedSeconds);
        else
            Highscore.AddHighscore("Noname", (int)MoneyManager.highscorePoints, (int)DAS.TimeSystem.TimePassedSeconds);

        Highscore.SortHighscore();
        Highscore.SaveHighscore();
        lists.GetComponent<DisplayScoreList>().AnimateHighscoreList();
        inputField.readOnly = true;

        foreach(var item in buttons)
            item.gameObject.SetActive(true);
    }
}
