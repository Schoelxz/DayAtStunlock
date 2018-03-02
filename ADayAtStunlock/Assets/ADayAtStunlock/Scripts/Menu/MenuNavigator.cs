using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigator : MonoBehaviour {

    Dictionary<string, Canvas> canvasList = new Dictionary<string, Canvas>();
    Canvas[] canvases;
    [SerializeField] private Canvas m_mainMenu;

    private void Start()
    {
        m_mainMenu = transform.GetChild(0).GetComponent<Canvas>();

        InitializeCanvases();
        GoToMainMenu();
    }

    private void HideAllCanvases()
    {
        foreach (Canvas canvas in canvasList.Values)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    private void InitializeCanvases()
    {
        canvases = GetComponentsInChildren<Canvas>(true);

        foreach (Canvas canvas in canvases)
        {
            if(canvas != GetComponent<Canvas>())
                canvasList.Add(canvas.name,canvas);
        }
    }

    public void GoToScreen(Canvas canvas)
    {
        //go to targetCanvas, hide others
        HideAllCanvases();
        canvas.gameObject.SetActive(true);

    }
    public void GoToMainMenu()
    {
        //go to main menu, hide others.
        if (canvasList.ContainsKey(m_mainMenu.name))
        {
            HideAllCanvases();
            GoToScreen(m_mainMenu);
        }
    }
    public void ExitApplication()
    {
        Debug.Log("You have quit the application!");
        Application.Quit();
    }
    public void GotoGameScene()
    {
        //goes to the game scene
        Debug.Log("You've started up the game, enjoy!");
    }
}
