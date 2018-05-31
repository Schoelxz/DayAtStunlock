using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreListScreen : MonoBehaviour {
   
    private Text[] m_names, m_scores, m_times;

    public static HighscoreListScreen s_thisInstance;

    private InputField m_inputField;

    private Text m_playerScoreText;

    private Button[] m_buttons;

    private GameObject m_lists;

    private void Awake()
    {
        if(s_thisInstance == null)
            s_thisInstance = this;
        else
            Destroy(gameObject);

        //Get  highscore list
        m_lists = gameObject.transform.Find("LeftPanel").transform.Find("HighscoreList (1)").transform.Find("Lists").gameObject;
        m_names = m_lists.transform.Find("Names").gameObject.GetComponentsInChildren<Text>(true);

        m_scores = m_lists.transform.Find("Scores").gameObject.GetComponentsInChildren<Text>(true);

        m_times = m_lists.transform.Find("Times").gameObject.GetComponentsInChildren<Text>(true);

        m_inputField = gameObject.GetComponentInChildren<InputField>();
        m_inputField.contentType = InputField.ContentType.Alphanumeric;
        m_inputField.onEndEdit.AddListener(delegate { SaveName(); });

        m_playerScoreText = gameObject.transform.Find("RightPanel").transform.Find("PlayerScore").GetComponentInChildren<Text>(true);

        m_buttons = gameObject.transform.Find("RightPanel").GetComponentsInChildren<Button>(true);

        foreach (var item in m_names)
            item.text = "-";

        foreach (var item in m_scores)
            item.text = "-";

        foreach (var item in m_times)
            item.text = "-";

        foreach (var item in m_buttons)
            item.gameObject.SetActive(false);
    }

    public void DisplayHighscoreScreen()
    {
        s_thisInstance.gameObject.SetActive(true);
        m_inputField.readOnly = false;
        m_inputField.text = "";

        foreach (var item in m_buttons)
            item.gameObject.SetActive(false);

        m_playerScoreText.text = MoneyManager.highscorePoints.ToString("n0");
    }

    private void SaveName()
    {
        if(m_inputField.text.Length > 0)
            Highscore.AddHighscore(m_inputField.text, (int)MoneyManager.highscorePoints, (int)DAS.TimeSystem.TimePassedSeconds);
        else
            Highscore.AddHighscore("Noname", (int)MoneyManager.highscorePoints, (int)DAS.TimeSystem.TimePassedSeconds);

        Highscore.SortHighscore();
        Highscore.SaveHighscore();
        m_lists.GetComponent<DisplayScoreList>().AnimateHighscoreList();
        m_inputField.readOnly = true;

        foreach(var item in m_buttons)
            item.gameObject.SetActive(true);
    }
}
