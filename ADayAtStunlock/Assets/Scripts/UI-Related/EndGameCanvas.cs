using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndGameCanvas : MonoBehaviour
{
    static GameObject myInstance;
    static Text highscore;
    static RectTransform highscorePanel;

    private void Awake()
    {
        myInstance = MenuNavigator.s_menuHolder.transform.GetChild(5).gameObject;
    }

    // Use this for initialization
    void Start ()
    {
        highscorePanel = myInstance.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>();

        highscore = highscorePanel.GetChild(0).GetComponent<Text>();
    }

    public static void GameOver()
    {
        myInstance.SetActive(true);
        DAS.TimeSystem.PauseTime();
        highscore.text = MoneyManager.moneyEarned.ToString("C0");
    }


}
